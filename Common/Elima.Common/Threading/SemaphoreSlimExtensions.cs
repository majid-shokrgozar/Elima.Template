using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.Threading;

public static class SemaphoreSlimExtensions
{
    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim)
    {
        await semaphoreSlim.WaitAsync();
        return semaphoreSlim.GetDispose();
    }

    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        return semaphoreSlim.GetDispose();
    }

    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
    {
        if (await semaphoreSlim.WaitAsync(millisecondsTimeout))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout, CancellationToken cancellationToken)
    {
        if (await semaphoreSlim.WaitAsync(millisecondsTimeout, cancellationToken))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
    {
        if (await semaphoreSlim.WaitAsync(timeout))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public async static Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (await semaphoreSlim.WaitAsync(timeout, cancellationToken))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim)
    {
        semaphoreSlim.Wait();
        return semaphoreSlim.GetDispose();
    }

    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
    {
        semaphoreSlim.Wait(cancellationToken);
        return semaphoreSlim.GetDispose();
    }

    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
    {
        if (semaphoreSlim.Wait(millisecondsTimeout))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout, CancellationToken cancellationToken)
    {
        if (semaphoreSlim.Wait(millisecondsTimeout, cancellationToken))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
    {
        if (semaphoreSlim.Wait(timeout))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (semaphoreSlim.Wait(timeout, cancellationToken))
        {
            return semaphoreSlim.GetDispose();
        }

        throw new TimeoutException();
    }

    private static IDisposable GetDispose(this SemaphoreSlim semaphoreSlim)
    {
        return new DisposeAction<SemaphoreSlim>(static (semaphoreSlim) =>
        {
            semaphoreSlim.Release();
        }, semaphoreSlim);
    }
}
