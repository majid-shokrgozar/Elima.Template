using System;

namespace Elima.Common.Presentation;

[Serializable]
public record PagedAndSortedResultRequest : PagedResultRequest, IPagedAndSortedResultRequest
{
    public virtual string? Sorting { get; set; }
}

