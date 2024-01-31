using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public interface ISupportsSavingChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
