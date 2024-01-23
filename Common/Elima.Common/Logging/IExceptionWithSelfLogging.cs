using Microsoft.Extensions.Logging;

namespace Elima.Common.Logging;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}
