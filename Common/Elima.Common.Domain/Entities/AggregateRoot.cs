using System;

namespace Elima.Common.Domain.Entities;

[Serializable]
public abstract class AggregateRoot : BasicAggregateRoot,
    IHasConcurrencyStamp
{

    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>,
    IHasConcurrencyStamp
{
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot(TKey id)
        : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
}
