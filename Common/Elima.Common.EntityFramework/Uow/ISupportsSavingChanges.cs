using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Uow;

public interface ISupportsSavingChanges
{
  
    int SaveChanges();

    int SaveChanges(bool acceptAllChangesOnSuccess);

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// This method will call the DbContext <see cref="SaveChangesAsync(bool, CancellationToken)"/> method directly of EF Core, which doesn't apply concepts of abp.
    /// </summary>
    Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
}
