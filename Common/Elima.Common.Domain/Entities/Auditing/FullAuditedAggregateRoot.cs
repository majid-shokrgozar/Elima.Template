﻿using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Domain.Entities.Auditing;

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited aggregate roots.
/// </summary>
[Serializable]
public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAuditedObject
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual string? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }
}

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited aggregate roots.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual string? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }


    protected FullAuditedAggregateRoot(TKey id)
    : base(id)
    {

    }
}
