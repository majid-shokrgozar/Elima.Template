using System;
using System.Collections.Generic;
using System.Linq;

namespace Elima.Common.Results;

public class Result: IResult
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
    public IReadOnlyList<Error> Errors=> _errors ?? [];

    public static Result Success() => new(ResultStatus.Succeeded, null, null);
    public static Result Failure(Error error, params Error[] errors) => new(ResultStatus.Failed, [error,.. errors], null);
    public static Result InvalidRequest(List<ValidationError> validationErrors) => new(ResultStatus.BadRequest, null, validationErrors);
    public static Result Unauthorized() => new(ResultStatus.Unauthorized, [Error.Unauthorized], null);
    public static Result Forbidden() => new(ResultStatus.Forbidden, [Error.Forbidden], null);
    public static Result NotFound<TId>(TId id) where TId : struct
        => new(ResultStatus.NotFound, [Error.NotFound(new Dictionary<string, object>() { { "Id", id } })], null);
    public static Result NotFound<TId>(Error error, TId id) where TId : struct
    => new(ResultStatus.NotFound, [error], null);
    public static Result NotImplemented() => new(ResultStatus.NotImplemented, [Error.NotImplemented], null);
    public static Result Conflict() => new(ResultStatus.Conflict, [Error.Conflict], null);
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
    public new static Result<TValue> Failure(Error error, params Error[] errors) => new(ResultStatus.Failed, [error, ..errors], null);
    public new static Result<TValue> InvalidRequest(List<ValidationError> validationErrors) => new(ResultStatus.BadRequest, null, validationErrors);
    public new static Result<TValue> Unauthorized() => new(ResultStatus.Unauthorized, [Error.Unauthorized], null);
    public new static Result<TValue> Forbidden() => new(ResultStatus.Forbidden, [Error.Forbidden], null);
    public new static Result<TValue> NotFound<TId>(TId id) where TId : struct
        => new(ResultStatus.NotFound, [Error.NotFound(new Dictionary<string, object>() { { "Id", id } })], null);

    public new static Result<TValue> NotFound<TId>(Error error, TId id) where TId : struct
=> new(ResultStatus.NotFound, [error], null);

    public new static Result<TValue> NotImplemented() => new(ResultStatus.NotImplemented, [Error.NotImplemented], null);
    public new static Result<TValue> Conflict() => new(ResultStatus.Conflict, [Error.Conflict], null);

    public static implicit operator Result<TValue>(TValue? value) => Result<TValue>.Success(value);

}