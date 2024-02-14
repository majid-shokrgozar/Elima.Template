using System;
using System.Linq;
using System.Threading.Tasks;
using Elima.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elima.Common.EntityFramework.Repositories;

public static class EntityFrameworkCoreRepositoryExtensions
{
    public static IQueryable<TEntity> AsNoTrackingIf<TEntity>(this IQueryable<TEntity> queryable, bool condition)
        where TEntity : class, IEntity
    {
        return condition ? queryable.AsNoTracking() : queryable;
    }
}
