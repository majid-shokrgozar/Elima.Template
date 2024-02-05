using System.Security.Claims;
using System;
using JetBrains.Annotations;

namespace Elima.Common.Security.Authentication;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    [CanBeNull]
    string? Id { get; }

    bool HasUserId { get; }
}
