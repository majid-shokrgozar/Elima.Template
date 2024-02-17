using Elima.Common.Domain.Entities.Auditing.Contracts;
using System;

namespace Elima.Common.Application;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
/// It also has the <see cref="Creator"/> object as a DTO represents the user.
/// </summary>
/// <typeparam name="TUserDto">Type of the User DTO</typeparam>
[Serializable]
public abstract record CreationAuditedEntityWithUserDto<TUserDto> : CreationAuditedEntityDto, ICreationAuditedObject<TUserDto>
{
    public TUserDto? Creator { get; set; }
}


/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
/// It also has the <see cref="Creator"/> object as a DTO represents the user.
/// </summary>
/// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
/// <typeparam name="TUserDto">Type of the User DTO</typeparam>
[Serializable]
public abstract record CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : CreationAuditedEntityDto<TPrimaryKey>, ICreationAuditedObject<TUserDto>
{
    protected CreationAuditedEntityWithUserDto(CreationAuditedEntityDto<TPrimaryKey> original) : base(original)
    {
    }

    public TUserDto? Creator { get; set; }
}