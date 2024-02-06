using Microsoft.AspNetCore.Http;
using System.Net;

namespace Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;

public interface IHttpExceptionStatusCodeFinder
{
    HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception);
}
