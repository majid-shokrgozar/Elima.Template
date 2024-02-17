using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.Repositories;

namespace Elima.Common.EntityFramework.Specification.Repositories;

public interface ISpecificationCommandRepository<TEntity> :ICommandRepository<TEntity>
    where TEntity : class, IEntity
{
}

public interface ISpecificationCommandRepository<TEntity, TKey> : ICommandRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}
