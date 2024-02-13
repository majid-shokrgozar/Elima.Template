using System;
using System.Collections.Generic;

namespace Elima.Common.Presentation;

[Serializable]
public record ListResultResponse<T> 
{
    /// <inheritdoc />
    public IReadOnlyList<T> Items
    {
        get { return _items ?? (_items = new List<T>()); }
        set { _items = value; }
    }
    private IReadOnlyList<T>? _items;

    /// <summary>
    /// Creates a new <see cref="ListResultDto{T}"/> object.
    /// </summary>
    public ListResultResponse()
    {

    }

    /// <summary>
    /// Creates a new <see cref="ListResultDto{T}"/> object.
    /// </summary>
    /// <param name="items">List of items</param>
    public ListResultResponse(IReadOnlyList<T> items)
    {
        Items = items;
    }
}