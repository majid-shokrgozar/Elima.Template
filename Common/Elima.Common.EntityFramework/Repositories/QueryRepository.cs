using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Repositories;

public class QueryRepository<TDbContext, TEntity> : BaseRepository<TDbContext, TEntity>, IQueryRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    public QueryRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await(await GetQueryableAsync(cancellationToken)).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken)).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken)).Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken);

        return await queryable
            .OrderByIf<TEntity, IQueryable<TEntity>>(!sorting.IsNullOrWhiteSpace(), sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }
}

public class QueryRepository<TDbContext, TEntity, TKey> : QueryRepository<TDbContext, TEntity>, IQueryRepository< TEntity, TKey>
    where TDbContext : IEfCoreDbContext
     where TEntity : class, IEntity<TKey>
{
    public QueryRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public async virtual Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken)).FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }
}