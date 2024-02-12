using System.Collections.Generic;
using System.Linq;

namespace Elima.Common.Results;

public interface IResult
{
    ResultStatus Status { get; }
    IReadOnlyList<ValidationError> ValidationError { get; }
    IReadOnlyList<Error> Errors { get; }
    public Error? Error { get; }

    public bool IsSuccess { get; }
    public bool IsFailure { get; }
}

public interface IResultWithValue : IResult
{
    object? GetValue();
}
