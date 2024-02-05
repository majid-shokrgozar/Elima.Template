using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elima.Common.DependencyInjection;
using Elima.Common.Domain.Entities;
using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.Domain.Entities.Events;
using Elima.Common.EntityFramework.Auditing;
using Elima.Common.EntityFramework.Data;
using Elima.Common.EntityFramework.EntityFrameworkCore.Modeling;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Reflection;
using Elima.Common.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Elima.Common.EntityFramework.EntityFrameworkCore;

public abstract class EfCoreDbContext<TDbContext> : DbContext, IEfCoreDbContext, ITransientDependency
    where TDbContext : DbContext
{
    protected EfCoreDbContext(
        DbContextOptions<TDbContext> options,
        ILogger<EfCoreDbContext<TDbContext>> logger,
        IAuditPropertySetter auditPropertySetter, 
        IDataFilter dataFilter 
        )
        : base(options)
    {
        _logger = logger;
        _auditPropertySetter = auditPropertySetter;
        _dataFilter = dataFilter;

        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
        ChangeTracker.Tracked += ChangeTracker_Tracked;
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }

    public readonly IAuditPropertySetter _auditPropertySetter;
    public readonly ILogger<EfCoreDbContext<TDbContext>> _logger;
    public readonly IDataFilter _dataFilter;
    protected virtual bool IsSoftDeleteFilterEnabled => _dataFilter?.IsEnabled<ISoftDelete>() ?? false;
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await this.Database.CommitTransactionAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        try
        {
            HandlePropertiesBeforeSave();

            var eventReport = CreateEventReport();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            PublishEntityEvents(eventReport);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (ex.Entries.Count > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine(ex.Entries.Count > 1
                    ? "There are some entries which are not saved due to concurrency exception:"
                    : "There is an entry which is not saved due to concurrency exception:");
                foreach (var entry in ex.Entries)
                {
                    sb.AppendLine(entry.ToString());
                }

                _logger.LogWarning(sb.ToString());
            }

            throw new DbConcurrencyException(ex.Message, ex);
        }
        finally
        {
            ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBasePropertiesMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });

            ConfigureValueConverterMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });

            ConfigureValueGeneratedMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
        }
    }

    /// <summary>
    /// This method will call the DbContext <see cref="SaveChangesAsync(bool, CancellationToken)"/> method directly of EF Core, which doesn't apply concepts of abp.
    /// </summary>
    public virtual Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry)
    {
        CheckAndSetId(entry);
        SetConcurrencyStampIfNull(entry);
        SetCreationAuditProperties(entry);
    }

    protected virtual void ApplyConceptsForDeletedEntity(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDelete)
        {
            return;
        }

        entry.Reload();
        ObjectHelper.TrySetProperty(entry.Entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
        SetDeletionAuditProperties(entry);
    }

    protected virtual void ApplyConceptsForModifiedEntity(EntityEntry entry)
    {
        if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.IsModified && (x.Metadata.ValueGenerated == ValueGenerated.Never || x.Metadata.ValueGenerated == ValueGenerated.OnAdd)))
        {
            IncrementEntityVersionProperty(entry);
            SetModificationAuditProperties(entry);

            if (entry.Entity is ISoftDelete && entry.Entity.As<ISoftDelete>().IsDeleted)
            {
                SetDeletionAuditProperties(entry);
            }
        }
    }

    protected virtual void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        ApplyConceptEntity(e.Entry);
    }

    protected virtual void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        ApplyConceptEntity(e.Entry);
    }

    protected virtual void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entry, entityWithGuidId);
        }
    }

    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.IsOwned())
        {
            return;
        }

        if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
        {
            return;
        }

        modelBuilder.Entity<TEntity>().ConfigureByConvention();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }

    protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
        {
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
            {
                modelBuilder.Entity<TEntity>().HasElimaQueryFilter(filterExpression);
            }
        }
    }

    protected virtual void ConfigureValueGenerated<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (!typeof(IEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
        {
            return;
        }

        var idPropertyBuilder = modelBuilder.Entity<TEntity>().Property(x => ((IEntity<Guid>)x).Id);
        if (idPropertyBuilder.Metadata.PropertyInfo!.IsDefined(typeof(DatabaseGeneratedAttribute), true))
        {
            return;
        }

        idPropertyBuilder.ValueGeneratedNever();
    }

    protected virtual EntityEventReport CreateEventReport()
    {
        var eventReport = new EntityEventReport();

        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            var generatesDomainEventsEntity = entry.Entity as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                continue;
            }

            var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
            if (localEvents != null && localEvents.Any())
            {
                eventReport.DomainEvents.AddRange(
                    localEvents.Select(
                        eventRecord => new DomainEventEntry(
                            entry.Entity,
                            eventRecord.EventData,
                            eventRecord.EventOrder
                        )
                    )
                );
                generatesDomainEventsEntity.ClearLocalEvents();
            }

            var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
            if (distributedEvents != null && distributedEvents.Any())
            {
                eventReport.DistributedEvents.AddRange(
                    distributedEvents.Select(
                        eventRecord => new DomainEventEntry(
                            entry.Entity,
                            eventRecord.EventData,
                            eventRecord.EventOrder)
                    )
                );
                generatesDomainEventsEntity.ClearDistributedEvents();
            }
        }

        return eventReport;
    }

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
        where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            expression = e => !IsSoftDeleteFilterEnabled || !EF.Property<bool>(e, "IsDeleted");
        }

        return expression;
    }

    protected virtual void HandlePropertiesBeforeSave()
    {
        foreach (var entry in from entry in ChangeTracker.Entries().ToList()
                              where entry.State.IsIn(EntityState.Modified, EntityState.Deleted)
                              select entry)
        {
            UpdateConcurrencyStamp(entry);
        }
    }

    protected virtual void IncrementEntityVersionProperty(EntityEntry entry)
    {
        _auditPropertySetter?.IncrementEntityVersionProperty(entry.Entity);
    }

    protected virtual void SetConcurrencyStampIfNull(EntityEntry entry)
    {
        var entity = entry.Entity as IHasConcurrencyStamp;
        if (entity == null)
        {
            return;
        }

        if (entity.ConcurrencyStamp != null)
        {
            return;
        }

        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected virtual void SetCreationAuditProperties(EntityEntry entry)
    {
        _auditPropertySetter?.SetCreationProperties(entry.Entity);
    }

    protected virtual void SetDeletionAuditProperties(EntityEntry entry)
    {
        _auditPropertySetter?.SetDeletionProperties(entry.Entity);
    }

    protected virtual void SetModificationAuditProperties(EntityEntry entry)
    {
        _auditPropertySetter?.SetModificationProperties(entry.Entity);
    }

    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return false;
    }

    protected virtual void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        var idProperty = entry.Property("Id").Metadata.PropertyInfo!;

        //Check for DatabaseGeneratedAttribute
        var dbGeneratedAttr = ReflectionHelper
            .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                idProperty
            );

        if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
        {
            return;
        }

        EntityHelper.TrySetId(
            entity,
            () => Guid.NewGuid(),
            true
        );
    }

    protected virtual void UpdateConcurrencyStamp(EntityEntry entry)
    {
        var entity = entry.Entity as IHasConcurrencyStamp;
        if (entity == null)
        {
            return;
        }

        Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
        entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected virtual void ConfigureValueConverter<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
       where TEntity : class
    {
        //if (mutableEntityType.BaseType == null &&
        //    !typeof(TEntity).IsDefined(typeof(DisableDateTimeNormalizationAttribute), true) &&
        //    !typeof(TEntity).IsDefined(typeof(OwnedAttribute), true) &&
        //    !mutableEntityType.IsOwned())
        //{
        //    if (LazyServiceProvider == null || Clock == null)
        //    {
        //        return;
        //    }

        //    foreach (var property in mutableEntityType.GetProperties().
        //                 Where(property => property.PropertyInfo != null &&
        //                                   (property.PropertyInfo.PropertyType == typeof(DateTime) || property.PropertyInfo.PropertyType == typeof(DateTime?)) &&
        //                                   property.PropertyInfo.CanWrite &&
        //                                   ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(property.PropertyInfo) == null))
        //    {
        //        modelBuilder
        //            .Entity<TEntity>()
        //            .Property(property.Name)
        //            .HasConversion(property.ClrType == typeof(DateTime)
        //                ? new AbpDateTimeValueConverter(Clock)
        //                : new AbpNullableDateTimeValueConverter(Clock));
        //    }
        //}
    }

    private void ApplyConceptEntity(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                ApplyConceptsForAddedEntity(entry);
                break;

            case EntityState.Modified:
                ApplyConceptsForModifiedEntity(entry);
                break;

            case EntityState.Deleted:
                ApplyConceptsForDeletedEntity(entry);
                break;
        }
    }

    private void PublishEntityEvents(EntityEventReport changeReport)
    {
        //Todo :

        //foreach (var localEvent in changeReport.DomainEvents)
        //{
        //    UnitOfWorkManager.Current?.AddOrReplaceLocalEvent(
        //        new UnitOfWorkEventRecord(localEvent.EventData.GetType(), localEvent.EventData, localEvent.EventOrder)
        //    );
        //}

        //foreach (var distributedEvent in changeReport.DistributedEvents)
        //{
        //    UnitOfWorkManager.Current?.AddOrReplaceDistributedEvent(
        //        new UnitOfWorkEventRecord(distributedEvent.EventData.GetType(), distributedEvent.EventData, distributedEvent.EventOrder)
        //    );
        //}
    }

    private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
        = typeof(EfCoreDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureBaseProperties),
                BindingFlags.Instance | BindingFlags.NonPublic
            )!;

    private static readonly MethodInfo ConfigureValueConverterMethodInfo
        = typeof(EfCoreDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureValueConverter),
                BindingFlags.Instance | BindingFlags.NonPublic
            )!;

    private static readonly MethodInfo ConfigureValueGeneratedMethodInfo
        = typeof(EfCoreDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureValueGenerated),
                BindingFlags.Instance | BindingFlags.NonPublic
            )!;
}