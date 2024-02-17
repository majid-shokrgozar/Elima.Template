using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Application;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObject"/> interface.
/// </summary>
[Serializable]
public abstract record FullAuditedEntityDto : AuditedEntityDto, IFullAuditedObject
{
    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public string? DeleterId { get; set; }

    /// <inheritdoc />
    public DateTime? DeletionTime { get; set; }
}

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObject"/> interface.
/// </summary>
/// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
[Serializable]
public abstract record FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IFullAuditedObject
{
    protected FullAuditedEntityDto(AuditedEntityDto<TPrimaryKey> original) : base(original)
    {
    }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public string? DeleterId { get; set; }

    /// <inheritdoc />
    public DateTime? DeletionTime { get; set; }
}