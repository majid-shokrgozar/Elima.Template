﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using JetBrains.Annotations;

namespace Elima.Common.EntityFramework.Repositories;

public interface ICommandRepository<TDbContext,TEntity> :IBasicRepository<TDbContext,TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    Task<TEntity> InsertAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(
    [NotNull] Expression<Func<TEntity, bool>> predicate,
    CancellationToken cancellationToken = default
    );

    Task DeleteDirectAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}

public interface ICommandRepository<TDbContext,TEntity, TKey> : ICommandRepository<TDbContext,TEntity>,IBasicRepository<TDbContext,TEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<TKey>
{
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}
