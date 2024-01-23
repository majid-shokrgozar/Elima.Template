using System;

namespace Elima.Common.Tracing;

public interface ICorrelationIdProvider
{
    string? Get();

    IDisposable Change(string? correlationId);
}
