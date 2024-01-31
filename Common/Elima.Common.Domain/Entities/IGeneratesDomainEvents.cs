using System.Collections.Generic;

namespace Elima.Common.Domain.Entities;

//TODO: Re-consider this interface

public interface IGeneratesDomainEvents
{
    IEnumerable<DomainEventRecord> GetLocalEvents();

    IEnumerable<DomainEventRecord> GetDistributedEvents();

    void ClearLocalEvents();

    void ClearDistributedEvents();
}
