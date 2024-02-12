using System;
using System.Collections.Generic;
using System.Linq;

namespace Elima.Common.Results;

public class Result : IResult
{
    private List<ValidationError>? _validationErrors = [];
    private List<Error>? _errors = [];
    protected internal Result(ResultStatus status, List<Error>? errors, List<ValidationError>? validationErrors)
    {
        Status = status;
        _errors = errors;
        _validationErrors = validationErrors;
    }

    public ResultStatus Status { get; }
    public bool IsSuccess => Status == ResultStatus.Succeeded;
    public bool IsFailure => !IsSuccess;
    public Error? Error => Errors.FirstOrDefault();
    public IReadOnlyList<ValidationError> ValidationError => _validationErrors ?? [];
    public IReadOnlyList<Error> Errors => _errors ?? [];

    public static Result Success() => new(ResultStatus.Succeeded, null, null);
    public static Result Failure(Error error, params Error[] errors) => new(ResultStatus.Failed, [error, .. errors], null);
    public static Result InvalidRequest(List<ValidationError> validationErrors) => new(ResultStatus.BadRequest, null, validationErrors);
    public static Result Unauthorized(Error? error = null) => new(ResultStatus.Unauthorized, [error ?? Error.Unauthorized], null);
    public static Result Forbidden(Error? error = null) => new(ResultStatus.Forbidden, [error ?? Error.Forbidden], null);
    public static Result NotFound<TId>(TId id) where TId : struct
        => new(ResultStatus.NotFound, [Error.NotFound(new Dictionary<string, object>() { { "Id", id } })], null);
    public static Result NotFound(Error error)
    => new(ResultStatus.NotFound, [error], null);
    public static Result NotImplemented(Error? error = null) => new(ResultStatus.NotImplemented, [error ?? Error.NotImplemented], null);
    public static Result Conflict(Error? error = null) => new(ResultStatus.Conflict, [error ?? Error.Conflict], null);
}

public class Result<TValue> : Result, IResultWithValue
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, ResultStatus isSuccess, List<Error>? errors, List<ValidationError>? validationErrors)
        : base(isSuccess, errors, validationErrors)
    {
        _value = value;
    }

    protected internal Result(ResultStatus isSuccess, List<Error>? errors, List<ValidationError>? validationErrors)
        : base(isSuccess, errors, validationErrors)
    {
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public object? GetValue()
    {
        return Value;
    }

    public static Result<TValue> Success(TValue? value) => new(value, ResultStatus.Succeeded, null, null);
    public new static Result<TValue> Failure(Error error, params Error[] errors) => new(ResultStatus.Failed, [error, .. errors], null);
    public new static Result<TValue> InvalidRequest(List<ValidationError> validationErrors) => new(ResultStatus.BadRequest, null, validationErrors);
    public new static Result<TValue> Unauthorized(Error? error = null) => new(ResultStatus.Unauthorized, [error??Error.Unauthorized], null);
    public new static Result<TValue> Forbidden(Error? error = null) => new(ResultStatus.Forbidden, [error??Error.Forbidden], null);
    public new static Result<TValue> NotFound<TId>(TId id) where TId : struct
        => new(ResultStatus.NotFound, [Error.NotFound(new Dictionary<string, object>() { { "Id", id } })], null);

    public new static Result<TValue> NotFound(Error error)
=> new(ResultStatus.NotFound, [error], null);

    public new static Result<TValue> NotImplemented(Error? error = null) => new(ResultStatus.NotImplemented, [error ?? Error.NotImplemented], null);
    public new static Result<TValue> Conflict(Error? error = null) => new(ResultStatus.Conflict, [error??Error.Conflict], null);

    public static implicit operator Result<TValue>(TValue? value) => Result<TValue>.Success(value);

}


public class ListResult<TValue> : Result<ListResultDto<TValue>>, IResultWithValue
{
    protected internal ListResult(ListResultDto<TValue>? value, ResultStatus isSuccess, List<Error>? errors, List<ValidationError>? validationErrors) : base(value, isSuccess, errors, validationErrors)
    {
    }
}

public class PagedResult<TValue> : Result<PagedResultDto<TValue>>, IResultWithValue
{
    protected internal PagedResult(PagedResultDto<TValue>? value, ResultStatus isSuccess, List<Error>? errors, List<ValidationError>? validationErrors) : base(value, isSuccess, errors, validationErrors)
    {
    }
}