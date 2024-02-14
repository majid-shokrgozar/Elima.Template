using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Elima.Common.EntityFramework.Repositories;

/// <summary>
/// Just to mark a class as repository.
/// </summary>
public interface IBaseRepository
{
}

public interface IBasicRepository<TEntity> :IBaseRepository
    where TEntity : class, IEntity
{
    Task<IEfCoreDbContext> GetDbContextAsync();

    Task<DbSet<TEntity>> GetDbSetAsync();

    Task<TEntity?> GetAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IDbConnection> GetDbConnectionAsync();

    Task<IDbTransaction?> GetDbTransactionAsync();

    Task EnsureCollectionLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken = default) where TProperty : class;

    Task EnsurePropertyLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> propertyExpression, CancellationToken cancellationToken = default) where TProperty : class;

    Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors);
}

public interface IBasicRepository< TEntity, TKey> : IBasicRepository< TEntity>
    where TEntity : class, IEntity<TKey>
{
    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);
}
