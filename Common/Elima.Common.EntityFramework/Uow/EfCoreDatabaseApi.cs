using Elima.Common.EntityFramework.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public class EfCoreDatabaseApi : IDatabaseApi, ISupportsSavingChanges
{
    public IEfCoreDbContext DbContext { get; }

    public EfCoreDatabaseApi(IEfCoreDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
