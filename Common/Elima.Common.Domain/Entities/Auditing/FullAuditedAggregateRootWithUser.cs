using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Domain.Entities.Auditing;

/// <summary>
/// Implements <see cref="IFullAuditedObject{TUser}"/> to be a base class for full-audited aggregate roots.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class FullAuditedAggregateRootWithUser<TUser> : FullAuditedAggregateRoot, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Deleter { get; set; }

    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }
}

/// <summary>
/// Implements <see cref="IFullAuditedObject{TUser}"/> to be a base class for full-audited aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class FullAuditedAggregateRootWithUser<TKey, TUser> : FullAuditedAggregateRoot<TKey>, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Deleter { get; set; }

    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }

    protected FullAuditedAggregateRootWithUser(TKey id)
        : base(id)
    {

    }
}
