using System;
using System.ComponentModel.DataAnnotations;

namespace Elima.Common.Application;

/// <summary>
/// Simply implements <see cref="IPagedResultRequest"/>.
/// </summary>
[Serializable]
public record BasePagedResultRequestDto : BaseLimitedResultRequestDto, IPagedResultRequest
{
    [Range(0, int.MaxValue)]
    public virtual int SkipCount { get; set; }
}
