
using System.Data;

namespace Elima.Common.EntityFramework.Uow;

public interface IUnitOfWorkOptions
{
    bool IsTransactional { get; }

    IsolationLevel? IsolationLevel { get; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    int? Timeout { get; }
}
