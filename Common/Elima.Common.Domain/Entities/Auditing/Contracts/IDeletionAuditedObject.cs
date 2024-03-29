﻿using System;

namespace Elima.Common.Domain.Entities.Auditing.Contracts;

/// <summary>
/// This interface can be implemented to store deletion information (who delete and when deleted).
/// </summary>
public interface IDeletionAuditedObject : IHasDeletionTime
{
    /// <summary>
    /// Id of the deleter user.
    /// </summary>
    string? DeleterId { get; }
}

/// <summary>
/// Extends <see cref="IDeletionAuditedObject"/> to add user navigation propery.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
public interface IDeletionAuditedObject<TUser> : IDeletionAuditedObject
{
    /// <summary>
    /// Reference to the deleter user.
    /// </summary>
    TUser? Deleter { get; }
}
