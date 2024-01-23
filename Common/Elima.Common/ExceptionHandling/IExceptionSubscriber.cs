using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Elima.Common.ExceptionHandling;

public interface IExceptionSubscriber
{
    Task HandleAsync([NotNull] ExceptionNotificationContext context);
}
