using Elima.Common.Domain.Entities;
using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.EntityFramework.Data;
using Elima.Common.System.Linq;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Repositories;

public abstract class RepositoryBase<TEntity> :  IRepository<TEntity>
    where TEntity : class, IEntity
{

    public IServiceProvider ServiceProvider { get; set; } = default!;
    public IDataFilter DataFilter { get; set; } = default!;

    public virtual Task<IQueryable<TEntity>> WithDetailsAsync()
    {
        return GetQueryableAsync();
    }

    public virtual Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return GetQueryableAsync();
    }

    public abstract Task<IQueryable<TEntity>> GetQueryableAsync();

    public abstract Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    public abstract Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

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

    public abstract Task<List<TEntity>> GetListAsync([NotNull] Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract Task<TEntity> InsertAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

    public abstract Task<TEntity> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

    public abstract Task DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    public abstract Task<long> GetCountAsync(CancellationToken cancellationToken = default);
    
    public abstract Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);
}

public abstract class RepositoryBase<TEntity, TKey> : RepositoryBase<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public abstract Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    public abstract Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, cancellationToken);
    }

}
