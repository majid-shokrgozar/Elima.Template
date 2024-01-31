using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
