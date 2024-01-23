using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Elima.Common.ExceptionHandling;

public interface IExceptionNotifier
{
    Task NotifyAsync([NotNull] ExceptionNotificationContext context);
}
