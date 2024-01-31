using System;

namespace Elima.Common.Domain.Entities.Auditing.Contracts;

/// <summary>
/// A standard interface to add DeletionTime property to a class.
/// It also makes the class soft delete (see <see cref="ISoftDelete"/>).
/// </summary>
public interface IHasDeletionTime : ISoftDelete
{
    /// <summary>
    /// Deletion time.
    /// </summary>
    DateTime? DeletionTime { get; }
}
