using System.Threading;

namespace Elima.Common.Domain.Entities;

public static class EventOrderGenerator
{
    private static long _lastOrder;

    public static long GetNext()
    {
        return Interlocked.Increment(ref _lastOrder);
    }
}