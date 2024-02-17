using Ardalis.Specification;
using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Repositories;
using Elima.Common.EntityFramework.Specification.Evaluators;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Repositories;

public abstract class SpecificationCommandRepository<TDbContext, TEntity> : CommandRepository<TDbContext, TEntity>, ICommandRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{

    private readonly ISpecificationEvaluator _specificationEvaluator;
    protected SpecificationCommandRepository(TDbContext dbContext)
        : this(dbContext, SpecificationEvaluator.Default)
    {
    }

    protected SpecificationCommandRepository(TDbContext dbContext, ISpecificationEvaluator specificationEvaluator)
        : base(dbContext)
    {
        _specificationEvaluator = specificationEvaluator;
    }

    public virtual void DeleteRange(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        GetDbContext().Set<TEntity>().RemoveRange(query);
    }

    /// <summary>
    /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
    /// <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false)
    {
        return _specificationEvaluator.GetQuery(GetDbContext().Set<TEntity>().AsQueryable(), specification, evaluateCriteriaOnly);
    }

    /// <summary>
    /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// <para>
    /// Projects each entity into a new form, being <typeparamref name="TResult" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return _specificationEvaluator.GetQuery(GetDbContext().Set<TEntity>().AsQueryable(), specification);
    }
}


public abstract class SpecificationCommandRepository<TDbContext, TEntity, TKey> : SpecificationCommandRepository<TDbContext, TEntity>, ICommandRepository<TEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, Domain.Entities.IEntity<TKey>
{
    protected SpecificationCommandRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await GetAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

}
