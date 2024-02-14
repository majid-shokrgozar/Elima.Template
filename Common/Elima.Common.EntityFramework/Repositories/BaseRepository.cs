using Elima.Common.Domain.Entities;
using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.EntityFramework.Data;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Repositories;

public abstract class BaseRepository<TDbContext, TEntity> : IBasicRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    protected BaseRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private readonly TDbContext _dbContext;
    public IServiceProvider ServiceProvider { get; set; } = default!;
    public IDataFilter DataFilter { get; set; } = default!;

    async Task<IEfCoreDbContext> IBasicRepository<TEntity>.GetDbContextAsync()
    {
      return (await GetDbContextAsync());
    }
    public Task<TDbContext> GetDbContextAsync()
    {
        return Task.FromResult(_dbContext);
    }

    public async Task<DbSet<TEntity>> GetDbSetAsync()
    {
        return (await GetDbContextAsync()).Set<TEntity>();
    }

    public async Task<IDbConnection> GetDbConnectionAsync()
    {
        return (await GetDbContextAsync()).Database.GetDbConnection();
    }

    public async Task<IDbTransaction?> GetDbTransactionAsync()
    {
        return (await GetDbContextAsync()).Database.CurrentTransaction?.GetDbTransaction();
    }

    public async virtual Task<TEntity?> GetAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async virtual Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return IncludeDetails(
                await GetQueryableAsync(),
                propertySelectors
        );
    }

    public async virtual Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbSetAsync()).AsQueryable();
    }

    public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
    {
        await (await GetDbContextAsync())
            .Entry(entity)
            .Collection(propertyExpression)
            .LoadAsync(cancellationToken);
    }

    public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty?>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
    {
        await (await GetDbContextAsync())
            .Entry(entity)
            .Reference(propertyExpression)
            .LoadAsync(cancellationToken);
    }

    protected virtual TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
        where TQueryable : IQueryable<TEntity>
    {
        return ApplyDataFilters<TQueryable, TEntity>(query);
    }

    protected virtual TQueryable ApplyDataFilters<TQueryable, TOtherEntity>(TQueryable query)
        where TQueryable : IQueryable<TOtherEntity>
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TOtherEntity)))
        {
            query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<ISoftDelete>(), e => ((ISoftDelete)e!).IsDeleted == false);
        }

        return query;
    }

    private static IQueryable<TEntity> IncludeDetails(
                IQueryable<TEntity> query,
                Expression<Func<TEntity, object>>[] propertySelectors)
    {
        if (!propertySelectors.IsNullOrEmpty())
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
        }

        return query;
    }

}

public abstract class BaseRepository<TDbContext, TEntity, TKey> : BaseRepository<TDbContext, TEntity>, IBasicRepository<TEntity, TKey>
     where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    protected BaseRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public async virtual Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken)).FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

}
