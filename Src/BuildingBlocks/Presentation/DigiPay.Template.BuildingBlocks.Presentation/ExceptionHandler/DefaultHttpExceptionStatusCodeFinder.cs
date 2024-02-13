using Elima.Common.DependencyInjection;
using Elima.Common.ExceptionHandling;
using Elima.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.BuildingBlocks.Presentation.ExceptionHandler;

public class DefaultHttpExceptionStatusCodeFinder : IHttpExceptionStatusCodeFinder, ITransientDependency
{
    protected ExceptionHttpStatusCodeOptions Options { get; }

    public DefaultHttpExceptionStatusCodeFinder(
        IOptions<ExceptionHttpStatusCodeOptions> options)
    {
        Options = options.Value;
    }

    public virtual HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
    {
        if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode &&
            exceptionWithHttpStatusCode.HttpStatusCode > 0)
        {
            return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;
        }

        if (exception is IHasErrorCode exceptionWithErrorCode &&
            !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
        {
            if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code!, out var status))
            {
                return status;
            }
        }

        //if (exception is AbpAuthorizationException)
        //{
        //    return httpContext.User.Identity!.IsAuthenticated
        //        ? HttpStatusCode.Forbidden
        //        : HttpStatusCode.Unauthorized;
        //}

        //TODO: Handle SecurityException..?

        if (exception is ValidationException)
        {
            return HttpStatusCode.BadRequest;
        }

        //if (exception is EntityNotFoundException)
        //{
        //    return HttpStatusCode.NotFound;
        //}

        if (exception is DbConcurrencyException)
        {
            return HttpStatusCode.Conflict;
        }

        if (exception is NotImplementedException)
        {
            return HttpStatusCode.NotImplemented;
        }

        if (exception is IBusinessException)
        {
            return HttpStatusCode.Forbidden;
        }

        return HttpStatusCode.InternalServerError;
    }
}