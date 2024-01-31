using System;
using System.Threading.Tasks;
using Elima.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elima.Common.EntityFramework.Repositories;

public interface IEfCoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<DbContext> GetDbContextAsync();

    Task<DbSet<TEntity>> GetDbSetAsync();
}

public interface IEfCoreRepository<TEntity, TKey> : IEfCoreRepository<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{

}
