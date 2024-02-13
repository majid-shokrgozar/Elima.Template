using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Application;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
/// </summary>
[Serializable]
public abstract class AuditedEntityDto : CreationAuditedEntityDto, IAuditedObject
{
    /// <inheritdoc />
    public DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public string? LastModifierId { get; set; }
}


/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
/// </summary>
/// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
[Serializable]
public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IAuditedObject
{
    /// <inheritdoc />
    public DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public string? LastModifierId { get; set; }
}
