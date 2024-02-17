using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Application;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject"/> interface.
/// </summary>
[Serializable]
public abstract record CreationAuditedEntityDto : EntityDto, ICreationAuditedObject
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
public abstract record CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAuditedObject
{
    protected CreationAuditedEntityDto(EntityDto<TPrimaryKey> original) : base(original)
    {
    }

    /// <inheritdoc />
    public DateTime CreationTime { get; set; }

    /// <inheritdoc />
    public string? CreatorId { get; set; }
}