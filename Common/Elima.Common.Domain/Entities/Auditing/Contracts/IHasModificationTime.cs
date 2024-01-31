using System;

namespace Elima.Common.Domain.Entities.Auditing.Contracts;

/// <summary>
/// A standard interface to add DeletionTime property to a class.
/// </summary>
public interface IHasModificationTime
{
    /// <summary>
    /// The last modified time for this entity.
    /// </summary>
    DateTime? LastModificationTime { get; }
}
