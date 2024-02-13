﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elima.Common.Application;



/// <summary>
/// Simply implements <see cref="ILimitedResultRequest"/>.
/// </summary>
[Serializable]
public class BaseLimitedResultRequestDto : ILimitedResultRequest
{
    /// <summary>
    /// Default value: 10.
    /// </summary>
    public static int DefaultMaxResultCount { get; set; } = 10;

    /// <summary>
    /// Maximum possible value of the <see cref="MaxResultCount"/>.
    /// Default value: 1,000.
    /// </summary>
    public static int MaxMaxResultCount { get; set; } = 1000;

    /// <summary>
    /// Maximum result count should be returned.
    /// This is generally used to limit result count on paging.
    /// </summary>
    [Range(1, int.MaxValue)]
    public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
}