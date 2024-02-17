using Ardalis.Specification;
using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Repositories;
using Elima.Common.EntityFramework.Specification.Evaluators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Specification.Repositories;

public abstract class SpecificationQueryRepository<TDbContext, TEntity> : QueryRepository<TDbContext, TEntity>, ISpecificationQueryRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected SpecificationQueryRepository(TDbContext dbContext)
        : this(dbContext, SpecificationEvaluator.Default)
    {
    }

    protected SpecificationQueryRepository(TDbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext)
    {
        _specificationEvaluator = specificationEvaluator;
    }

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TResult?> GetFirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetSingleOrDefaultAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TResult?> GetSingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<List<TEntity>> GetListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    /// <inheritdoc/>
    public virtual async Task<List<TResult>> GetListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    /// <inheritdoc/>
    public virtual async Task<int> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification, true).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<long> GetLongCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification, true).LongCountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification, true).AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbContext().Set<TEntity>().AnyAsync(cancellationToken);
    }

    public virtual IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification)
    {
        return ApplySpecification(specification).AsAsyncEnumerable();
    }

    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false)
    {
        return _specificationEvaluator.GetQuery(GetQueryable(), specification, evaluateCriteriaOnly);
    }

    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return _specificationEvaluator.GetQuery(GetQueryable(), specification);
    }
}

public abstract class SpecificationQueryRepository<TDbContext, TEntity, TKey> : SpecificationQueryRepository<TDbContext, TEntity>, ISpecificationQueryRepository<TEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, Domain.Entities.IEntity<TKey>
{
    protected SpecificationQueryRepository(TDbContext dbContext)
      : this(dbContext, SpecificationEvaluator.Default)
    {
    }
    protected SpecificationQueryRepository(TDbContext dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext, specificationEvaluator)
    {
    }

    public async virtual Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken)).FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }
}