using System.Collections.Generic;

namespace Elima.Common.Results;

public class PagedResult<TValue> : Result<PagedResultDto<TValue>>, IResultWithValue<PagedResultDto<TValue>>
{
    protected internal PagedResult(PagedResultDto<TValue>? value, ResultStatus isSuccess, List<Error>? errors, List<ValidationError>? validationErrors) : base(value, isSuccess, errors, validationErrors)
    {
    }

    public new static PagedResult<TValue> Success(PagedResultDto<TValue>? value) => new(value, ResultStatus.Succeeded, null, null);

    public static implicit operator PagedResult<TValue>(PagedResultDto<TValue>? value) => PagedResult<TValue>.Success(value);
}