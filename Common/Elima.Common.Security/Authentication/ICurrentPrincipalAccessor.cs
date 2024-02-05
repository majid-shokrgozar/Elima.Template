using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Elima.Common.Security.Authentication;

public interface ICurrentPrincipalAccessor
{
    ClaimsPrincipal Principal { get; }

    IDisposable Change(ClaimsPrincipal principal);
}
