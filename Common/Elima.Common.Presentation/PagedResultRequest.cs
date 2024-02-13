using System;
using System.ComponentModel.DataAnnotations;

namespace Elima.Common.Presentation;

[Serializable]
public record PagedResultRequest : LimitedResultRequest, IPagedResultRequest
{
    [Range(0, int.MaxValue)]
    public virtual int SkipCount { get; set; }
}
