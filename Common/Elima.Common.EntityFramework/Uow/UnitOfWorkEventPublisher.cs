using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public class UnitOfWorkEventPublisher : IUnitOfWorkEventPublisher
{
    public Task PublishDistributedEventsAsync(IEnumerable<UnitOfWorkEventRecord> distributedEvents)
    {
        throw new global::System.NotImplementedException();
    }

    public Task PublishLocalEventsAsync(IEnumerable<UnitOfWorkEventRecord> localEvents)
    {
        throw new global::System.NotImplementedException();
    }
}
