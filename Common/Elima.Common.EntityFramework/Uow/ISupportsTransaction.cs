using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public interface ISupportsTransaction 
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
