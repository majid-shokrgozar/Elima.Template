using System;

namespace Elima.Common.Domain.Entities.Auditing.Contracts;

public interface IMayHaveCreator<TCreator>
{
    /// <summary>
    /// Reference to the creator.
    /// </summary>
    TCreator? Creator { get; }
}

/// <summary>
/// Standard interface for an entity that MAY have a creator.
/// </summary>
public interface IMayHaveCreator
{
    /// <summary>
    /// Id of the creator.
    /// </summary>
    string? CreatorId { get; }
}
