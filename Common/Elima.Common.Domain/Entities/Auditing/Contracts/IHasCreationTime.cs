using System;

namespace Elima.Common.Domain.Entities.Auditing.Contracts;

/// <summary>
/// A standard interface to add CreationTime property.
/// </summary>
public interface IHasCreationTime
{
    /// <summary>
    /// Creation time.
    /// </summary>
    DateTime CreationTime { get; }
}
