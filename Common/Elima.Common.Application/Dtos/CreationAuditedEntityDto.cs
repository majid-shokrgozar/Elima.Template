using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Application;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject"/> interface.
/// </summary>
[Serializable]
public abstract class CreationAuditedEntityDto : EntityDto, ICreationAuditedObject
{
    /// <inheritdoc />
    public DateTime CreationTime { get; set; }

    /// <inheritdoc />
    public string? CreatorId { get; set; }
}

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject"/> interface.
/// </summary>
/// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
[Serializable]
public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAuditedObject
{
    /// <inheritdoc />
    public DateTime CreationTime { get; set; }

    /// <inheritdoc />
    public string? CreatorId { get; set; }
}