using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Elima.Common.EntityFramework.Uow;

public interface IUnitOfWork : IDisposable
{
    Guid Id { get; }

    //TODO: Switch to OnFailed (sync) and OnDisposed (sync) methods to be compatible with OnCompleted
    event EventHandler<UnitOfWorkFailedEventArgs> Failed;

    event EventHandler<UnitOfWorkEventArgs> Disposed;

    IUnitOfWork? Outer { get; }

    bool IsDisposed { get; }

    bool IsCompleted { get; }

    void SetOuter(IUnitOfWork? outer);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task CompleteAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    void OnCompleted(Func<Task> handler);

    void AddOrReplaceLocalEvent(
        UnitOfWorkEventRecord eventRecord,
        Predicate<UnitOfWorkEventRecord>? replacementSelector = null
    );

    void AddOrReplaceDistributedEvent(
        UnitOfWorkEventRecord eventRecord,
        Predicate<UnitOfWorkEventRecord>? replacementSelector = null
    );
}
