using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace Elima.Common.Security.Authentication;

public interface IAuthorizationExceptionHandler
{
    Task HandleAsync(AuthorizationException exception, HttpContext httpContext);
}
