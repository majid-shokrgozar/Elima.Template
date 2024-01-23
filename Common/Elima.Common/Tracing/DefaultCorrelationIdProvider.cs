using Elima.Common.DependencyInjection;
using System;
using System.Threading;

namespace Elima.Common.Tracing;

public class DefaultCorrelationIdProvider : ICorrelationIdProvider, ISingletonDependency
{
    private readonly AsyncLocal<string?> _currentCorrelationId = new AsyncLocal<string?>();

    private string? CorrelationId => _currentCorrelationId.Value;

    public virtual string? Get()
    {
        return CorrelationId;
    }

    public virtual IDisposable Change(string? correlationId)
    {
        var parent = CorrelationId;
        _currentCorrelationId.Value = correlationId;
        return new DisposeAction(() =>
        {
            _currentCorrelationId.Value = parent;
        });
    }
}
