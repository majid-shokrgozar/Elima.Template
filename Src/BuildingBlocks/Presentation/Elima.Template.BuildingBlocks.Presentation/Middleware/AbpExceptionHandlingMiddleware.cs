using Elima.Common.ExceptionHandling;
using Elima.Common.Security.Authentication;
using Elima.Common.System;
using Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Elima.Template.BuildingBlocks.Presentation.Middleware;

public class AbpExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<AbpExceptionHandlingMiddleware> _logger;

    private readonly Func<object, Task> _clearCacheHeadersDelegate;

    public AbpExceptionHandlingMiddleware(ILogger<AbpExceptionHandlingMiddleware> logger)
    {
        _logger = logger;

        _clearCacheHeadersDelegate = ClearCacheHeaders;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("An exception occurred, but response has already started!");
                throw;
            }

            await HandleAndWrapException(context, ex);
            return;

        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception.Message,exception);

        //await httpContext
        //    .RequestServices
        //    .GetRequiredService<IExceptionNotifier>()
        //    .NotifyAsync(
        //        new ExceptionNotificationContext(exception)
        //    );

        if (exception is AuthorizationException)
        {
            await httpContext.RequestServices.GetRequiredService<IAuthorizationExceptionHandler>()
                .HandleAsync(exception.As<AuthorizationException>(), httpContext);
        }
        else
        {
            var errorInfoConverter = httpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var statusCodeFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            var exceptionHandlingOptions = httpContext.RequestServices.GetRequiredService<IOptions<ExceptionHandlingOptions>>().Value;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)statusCodeFinder.GetStatusCode(httpContext, exception);
            httpContext.Response.OnStarting(_clearCacheHeadersDelegate, httpContext.Response);
            httpContext.Response.Headers.Append("Content-Type", "application/json");

            await httpContext.Response.WriteAsync(
               JsonSerializer .Serialize(
                    new ProblemDetailsResponse(
                        errorInfoConverter.Convert(exception, options =>
                        {
                            options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                            options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
                        })
                    )
                )
            );
        }
    }

    private Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    }
}
