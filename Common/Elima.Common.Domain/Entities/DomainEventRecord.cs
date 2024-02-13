using System;

namespace Elima.Common.Domain.Entities;

public class DomainEventRecord
{
    public object EventData { get; }

    public long EventOrder { get; }

    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;

    public DomainEventRecord(object eventData, long eventOrder)
    {
        EventData = eventData;
        EventOrder = eventOrder;
    }
}
