﻿using Elima.Common.DependencyInjection;
using Elima.Common.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace Elima.Template.WebApi.Authorization;

public class CurrentUser : ICurrentUser, ITransientDependency
{
    public virtual bool IsAuthenticated => !string.IsNullOrWhiteSpace(Id);

    public virtual string? Id => GetUserId(_httpContextAccessor);

    public bool HasUserId => !string.IsNullOrWhiteSpace(Id);

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserId(IHttpContextAccessor contextAccessor)
    {
        var headrs = contextAccessor?.HttpContext?.Request?.Headers;
        if (headrs != null && headrs.TryGetValue("UserId", out Microsoft.Extensions.Primitives.StringValues value))
        {
            return value!;
        }
        return "";
    }
}
