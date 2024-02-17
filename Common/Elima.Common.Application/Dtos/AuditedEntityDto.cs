using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Application;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
/// </summary>
[Serializable]
public abstract record AuditedEntityDto : CreationAuditedEntityDto, IAuditedObject
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
public abstract record AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IAuditedObject
{
    protected AuditedEntityDto(CreationAuditedEntityDto<TPrimaryKey> original) : base(original)
    {
    }

    /// <inheritdoc />
    public DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public string? LastModifierId { get; set; }
}
