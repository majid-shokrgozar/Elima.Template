using System;
using System.Collections.Generic;

namespace Elima.Common.Presentation;

[Serializable]
public record PagedResultResponse<T>: ListResultResponse<T>
{
    /// <inheritdoc />
    public long TotalCount { get; set; }

    /// <summary>
    /// Creates a new <see cref="PagedResultDto{T}"/> object.
    /// </summary>
    public PagedResultResponse()
    {

    }

    /// <summary>
    /// Creates a new <see cref="PagedResultDto{T}"/> object.
    /// </summary>
    /// <param name="totalCount">Total count of Items</param>
    /// <param name="items">List of items in current page</param>
    public PagedResultResponse(long totalCount, IReadOnlyList<T> items)
        : base(items)
    {
        TotalCount = totalCount;
    }
}
