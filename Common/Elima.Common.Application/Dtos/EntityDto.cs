using System;

namespace Elima.Common.Application;

[Serializable]
public abstract record EntityDto : IEntityDto //TODO: Consider to delete this class
{
    protected EntityDto()
    {
    }
    public override string ToString()
    {
        return $"[DTO: {GetType().Name}]";
    }
}

[Serializable]
public abstract record EntityDto<TKey> : EntityDto, IEntityDto<TKey>
{
    /// <summary>
    /// Id of the entity.
    /// </summary>
    public TKey Id { get; set; } = default!;

    protected EntityDto(TKey id) 
    {
        Id = id;
    }

    public override string ToString()
    {
        return $"[DTO: {GetType().Name}] Id = {Id}";
    }
}