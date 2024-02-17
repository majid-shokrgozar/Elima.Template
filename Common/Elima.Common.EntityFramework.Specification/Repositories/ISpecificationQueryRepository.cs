using Ardalis.Specification;
using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.Repositories;

namespace Elima.Common.EntityFramework.Specification.Repositories;

public interface ISpecificationQueryRepository<TEntity> : IQueryRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification);
    Task<int> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TEntity?> GetFirstOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TResult?> GetFirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<List<TResult>> GetListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);
    Task<long> GetLongCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TEntity?> GetSingleOrDefaultAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TResult?> GetSingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);
}

public interface ISpecificationQueryRepository<TEntity, TKey> : ISpecificationQueryRepository<TEntity>, IQueryRepository<TEntity,TKey>
    where TEntity : class, Domain.Entities.IEntity<TKey>
{
}