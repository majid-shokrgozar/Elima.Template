using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Elima.Common;

public interface IOnApplicationShutdown
{
    Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context);

    void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
}
