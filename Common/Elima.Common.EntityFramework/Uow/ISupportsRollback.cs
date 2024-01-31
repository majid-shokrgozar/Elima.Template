using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public interface ISupportsRollback
{
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
