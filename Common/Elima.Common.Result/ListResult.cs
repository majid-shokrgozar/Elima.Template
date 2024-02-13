using System.Collections.Generic;

namespace Elima.Common.Results;

public class ListResult<TValue> : Result<ListResultDto<TValue>>, IResultWithValue<ListResultDto<TValue>>
{
    protected internal ListResult(ListResultDto<TValue>? value, ResultStatus isSuccess, List<Error>? errors, List<ValidationError>? validationErrors) : base(value, isSuccess, errors, validationErrors)
    {
    }

    public new static ListResult<TValue> Success(ListResultDto<TValue>? value) => new(value, ResultStatus.Succeeded, null, null);

    public static implicit operator ListResult<TValue>(ListResultDto<TValue>? value) => ListResult<TValue>.Success(value);
}
