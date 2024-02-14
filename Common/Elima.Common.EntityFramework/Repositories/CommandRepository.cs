using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Repositories;

public class CommandRepository<TDbContext,TEntity> :BaseRepository<TDbContext,TEntity>, ICommandRepository<TDbContext,TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    public CommandRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task DeleteAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(cancellationToken);

        foreach (var entity in entities)
            await DeleteAsync(entity, cancellationToken);
    }

    public async Task DeleteDirectAsync([NotNull] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();
        await dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<TEntity> InsertAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;

        return savedEntity;
    }

    public async Task<TEntity> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        if (dbContext.Set<TEntity>().Local.All(e => e != entity))
        {
            dbContext.Set<TEntity>().Attach(entity);
            dbContext.Update(entity);
        }
        return entity;
    }
}


public class CommandRepository<TDbContext,TEntity, TKey> : CommandRepository<TDbContext,TEntity>, ICommandRepository<TDbContext,TEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    public CommandRepository(TDbContext dbContext) : base(dbContext)
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
        return await(await GetQueryableAsync()).FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

}
