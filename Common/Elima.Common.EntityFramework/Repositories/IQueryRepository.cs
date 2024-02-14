using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;

namespace Elima.Common.EntityFramework.Repositories;

public interface IQueryRepository<TDbContext,TEntity> : IBasicRepository<TDbContext,TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default);
}

public interface IQueryRepository<TDbContext,TEntity, TKey>: IQueryRepository<TDbContext,TEntity>, IBasicRepository<TDbContext,TEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
}
