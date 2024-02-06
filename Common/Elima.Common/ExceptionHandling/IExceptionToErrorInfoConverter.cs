using System;

namespace Elima.Common.ExceptionHandling;

public interface IExceptionToErrorInfoConverter
{
    /// <summary>
    /// Converter method.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="options">Additional options.</param>
    /// <returns>Error info or null</returns>
    ProblemDetails Convert(Exception exception, Action<ExceptionHandlingOptions>? options = null);
}