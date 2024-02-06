using Elima.Common.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Common.Security.Authentication;

public class DefaultAuthorizationExceptionHandler : IAuthorizationExceptionHandler, ITransientDependency
{
    public virtual async Task HandleAsync(AuthorizationException exception, HttpContext httpContext)
    {
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
        var authenticationSchemeProvider = httpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

        AuthenticationScheme? scheme;
        if (isAuthenticated)
        {
            scheme = await authenticationSchemeProvider.GetDefaultForbidSchemeAsync();
            if (scheme == null)
            {
                throw new ElimaException($"There was no DefaultForbidScheme found.");
            }
        }
        else
        {
            scheme = await authenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
            if (scheme == null)
            {
                throw new ElimaException($"There was no DefaultChallengeScheme found.");
            }
        }

        var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name);
        if (handler == null)
        {
            throw new ElimaException($"No handler of {scheme.Name} was found.");
        }

        if (isAuthenticated)
        {
            await handler.ForbidAsync(null);
        }
        else
        {
            await handler.ChallengeAsync(null);
        }
    }
}