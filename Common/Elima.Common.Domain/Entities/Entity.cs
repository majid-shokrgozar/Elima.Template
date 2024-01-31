using System;
using System.Collections.Generic;

namespace Elima.Common.Domain.Entities;

/// <inheritdoc/>
[Serializable]
public abstract class Entity : IEntity
{
    protected Entity()
    {
      
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
    }

    public abstract object?[] GetKeys();

    public bool EntityEquals(IEntity other)
    {
        return EntityHelper.EntityEquals(this, other);
    }
}

/// <inheritdoc cref="IEntity{TKey}" />
[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>, IEquatable<Entity<TKey>>
    where TKey : notnull
{
    /// <inheritdoc/>
    public  virtual TKey Id { get; protected set; } = default!;

    protected Entity()
    {

    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public override object?[] GetKeys()
    {
        return [Id];
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (GetType() != obj.GetType()) return false;

        if (obj is not Entity<TKey> entityBase)
        {
            return false;
        }
        return Id.Equals(entityBase.Id);
    }

    public bool Equals(Entity<TKey>? other)
    {
        if (other == null) return false;

        if (GetType() != other.GetType()) return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() + 31;
    }
}
