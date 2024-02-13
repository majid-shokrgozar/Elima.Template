using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Elima.Common.System;
using System.Collections.Immutable;
using Elima.Common.EntityFramework.EntityFrameworkCore;

namespace Elima.Common.EntityFramework.Uow;

public class UnitOfWork : IUnitOfWork
{
    public Guid Id { get; } = Guid.NewGuid();

    public IUnitOfWork? Outer { get; private set; }

    public bool IsReserved { get; set; }

    public bool IsDisposed { get; private set; }

    public bool IsCompleted { get; private set; }


    protected List<Func<Task>> CompletedHandlers { get; } = new List<Func<Task>>();
    protected List<UnitOfWorkEventRecord> DistributedEvents { get; } = new List<UnitOfWorkEventRecord>();
    protected List<UnitOfWorkEventRecord> LocalEvents { get; } = new List<UnitOfWorkEventRecord>();

    public event EventHandler<UnitOfWorkFailedEventArgs> Failed = default!;
    public event EventHandler<UnitOfWorkEventArgs> Disposed = default!;

    protected IUnitOfWorkEventPublisher UnitOfWorkEventPublisher { get; }

    private readonly IEnumerable<IEfCoreDbContext> _databases;

    private Exception? _exception;
    private bool _isCompleting;
    private bool _isRolledback;

    public UnitOfWork(
        //IUnitOfWorkEventPublisher unitOfWorkEventPublisher,
        IEnumerable<IEfCoreDbContext> databases)
    {
        // UnitOfWorkEventPublisher = unitOfWorkEventPublisher;
        _databases = databases;
    }

    public virtual void SetOuter(IUnitOfWork? outer)
    {
        Outer = outer;
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback)
        {
            return;
        }

        foreach (var databaseApi in GetAllActiveDatabases())
        {
            await databaseApi.SaveChangesAsync(cancellationToken);
        }
    }

    public virtual IReadOnlyList<IEfCoreDbContext> GetAllActiveDatabases()
    {
        return _databases.ToImmutableList();
    }

    public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback)
        {
            return;
        }

        PreventMultipleComplete();

        try
        {
            _isCompleting = true;
            await SaveChangesAsync(cancellationToken);

            while (LocalEvents.Any() || DistributedEvents.Any())
            {
                if (LocalEvents.Any())
                {
                    var localEventsToBePublished = LocalEvents.OrderBy(e => e.EventOrder).ToArray();
                    LocalEvents.Clear();
                    await UnitOfWorkEventPublisher.PublishLocalEventsAsync(
                        localEventsToBePublished
                    );
                }

                if (DistributedEvents.Any())
                {
                    var distributedEventsToBePublished = DistributedEvents.OrderBy(e => e.EventOrder).ToArray();
                    DistributedEvents.Clear();
                    await UnitOfWorkEventPublisher.PublishDistributedEventsAsync(
                        distributedEventsToBePublished
                    );
                }

                await SaveChangesAsync(cancellationToken);
            }

            await CommitTransactionsAsync(cancellationToken);
            IsCompleted = true;
            await OnCompletedAsync();
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback)
        {
            return;
        }

        _isRolledback = true;

        await RollbackAllAsync(cancellationToken);
    }

    public virtual void OnCompleted(Func<Task> handler)
    {
        CompletedHandlers.Add(handler);
    }

    public virtual void AddOrReplaceLocalEvent(
        UnitOfWorkEventRecord eventRecord,
        Predicate<UnitOfWorkEventRecord>? replacementSelector = null)
    {
        AddOrReplaceEvent(LocalEvents, eventRecord, replacementSelector);
    }

    public virtual void AddOrReplaceDistributedEvent(
        UnitOfWorkEventRecord eventRecord,
        Predicate<UnitOfWorkEventRecord>? replacementSelector = null)
    {
        AddOrReplaceEvent(DistributedEvents, eventRecord, replacementSelector);
    }

    public virtual void AddOrReplaceEvent(
        List<UnitOfWorkEventRecord> eventRecords,
        UnitOfWorkEventRecord eventRecord,
        Predicate<UnitOfWorkEventRecord>? replacementSelector = null)
    {
        if (replacementSelector == null)
        {
            eventRecords.Add(eventRecord);
        }
        else
        {
            var foundIndex = eventRecords.FindIndex(replacementSelector);
            if (foundIndex < 0)
            {
                eventRecords.Add(eventRecord);
            }
            else
            {
                eventRecords[foundIndex] = eventRecord;
            }
        }
    }

    protected virtual async Task OnCompletedAsync()
    {
        foreach (var handler in CompletedHandlers)
        {
            await handler.Invoke();
        }
    }

    protected virtual void OnFailed()
    {
        Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(this, _exception, _isRolledback));
    }

    protected virtual void OnDisposed()
    {
        Disposed.InvokeSafely(this, new UnitOfWorkEventArgs(this));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;

        DisposeTransactions();


        if (!IsCompleted || _exception != null)
        {
            OnFailed();
        }

        OnDisposed();
    }


    protected virtual async Task RollbackAllAsync(CancellationToken cancellationToken)
    {
        foreach (var databaseApi in GetAllActiveDatabases())
        {
            if (databaseApi is ISupportsRollback supportsRollbackDatabaseApi)
            {
                try
                {
                    await supportsRollbackDatabaseApi.RollbackAsync(cancellationToken);
                }
                catch
                {
                    //ignore
                }
            }
        }

        foreach (var transactionApi in GetAllActiveDatabases())
        {
            if (transactionApi is ISupportsRollback supportsRollbackTransactionApi)
            {
                try
                {
                    await supportsRollbackTransactionApi.RollbackAsync(cancellationToken);
                }
                catch
                {
                    //ignore}
                }
            }
        }
    }

    protected virtual async Task CommitTransactionsAsync(CancellationToken cancellationToken)
    {
        foreach (var transaction in GetAllActiveDatabases())
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    private void DisposeTransactions()
    {
        foreach (var transactionApi in GetAllActiveDatabases())
        {
            try
            {
                transactionApi.Dispose();
            }
            catch
            {
                //ignore
            }
        }
    }

    private void PreventMultipleComplete()
    {
        if (IsCompleted || _isCompleting)
        {
            throw new ElimaException("Completion has already been requested for this unit of work.");
        }
    }
    public override string ToString()
    {
        return $"[UnitOfWork {Id}]";
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}
