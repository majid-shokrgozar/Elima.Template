using Elima.Common.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProblemDetails = Elima.Common.ExceptionHandling.ProblemDetails;
namespace FastEndpoints;

public abstract class EndpointElimaResultWithMapping<TRequest, TResponse, TCommandOrQuery, TResultValue> : Endpoint<TRequest, TResponse>
    where TRequest : notnull
{
    public Task SendAsync(Result<TResultValue> result, Func<TResultValue, TResponse>? mapper, CancellationToken cancellationToken)
    {
        return result.Status switch
        {
            ResultStatus.Succeeded => SendSucceededAsync(result, mapper, cancellationToken),
            ResultStatus.BadRequest => SendBadRequestAsync(result, cancellationToken),
            ResultStatus.Unauthorized => SendUnauthorizedAsync(result, cancellationToken),
            ResultStatus.Forbidden => SendForbiddenAsync(result, cancellationToken),
            ResultStatus.NotFound => SendNotFoundAsync(result, cancellationToken),
            ResultStatus.Conflict => SendConflictAsync(result, cancellationToken),
            ResultStatus.Failed => SendFailedAsync(result, cancellationToken),
            ResultStatus.NotImplemented => SendNotImplementedAsync(result, cancellationToken),
            ResultStatus.Unprocessable => SendNotUnprocessableAsync(result, cancellationToken),
            _ => throw new NotImplementedException(),
        };
    }

    public Task SendAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        return SendAsync(result, null, cancellationToken);
    }

    private async Task SendNotUnprocessableAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendNotImplementedAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendFailedAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendConflictAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendNotFoundAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendForbiddenAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendUnauthorizedAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendBadRequestAsync(Result<TResultValue> result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResulToProblemDetails(result,"ValidationError",string.Empty,string.Empty), (int)result.Status, cancellationToken);
    }

    private async Task SendProblemDetailsAsync(Elima.Common.ExceptionHandling.ProblemDetails problemDetails, int httpStatus, CancellationToken cancellationToken)
    {
        await SendStringAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails), httpStatus, "application/json", cancellationToken);
    }

    private Elima.Common.ExceptionHandling.ProblemDetails ConvertResulToProblemDetails(Result<TResultValue> result, string defaultError, string instance,string type)
    {
        var problemDetails = new Elima.Common.ExceptionHandling.ProblemDetails();
        problemDetails.Title = result.Errors.Count == 1 ? result.Errors[0].Message : defaultError;
        problemDetails.Status = (int)result.Status;
        problemDetails.Type = type;
        problemDetails.Instance = instance;

        foreach (var error in result.Errors)
        {

            problemDetails.Details ??= [];

            problemDetails.Details.Add(new Elima.Common.ExceptionHandling.ProblemDetail()
            {
                Code = error.Code,
                Message = error.Message,
                Data = error.Data,
                Details = error.Exception?.Message,
                StackTrace = error.Exception?.StackTrace
            });
        }

        foreach (var error in result.ValidationError)
        {

            problemDetails.ValidationErrors ??= [];

            problemDetails.ValidationErrors.Add(new Elima.Common.ExceptionHandling.ValidationErrorInfo()
            {
                Members = [error.PropertyName],
                Message = error.ErrorMessage,
                Code=error.ErrorCode,
                AttemptedValue = error.AttemptedValue
            });
        }

        return problemDetails;
    }

    private async Task SendSucceededAsync(Result<TResultValue> result, Func<TResultValue, TResponse>? mapper, CancellationToken cancellationToken)
    {
        TResponse? response;

        if (mapper != null)
        {
            response = mapper.Invoke(result.Value);
        }
        else
        {
            response = await MapFromResultValueAsync(result.Value, cancellationToken);
        }

        await SendStringAsync(System.Text.Json.JsonSerializer.Serialize(response), 200, "application/json", cancellationToken);
    }

    /// <summary>
    /// override this method and place the logic for mapping the request dto to the desired domain entity
    /// </summary>
    /// <param name="r">the request dto</param>
    public virtual TCommandOrQuery MapToCommandOrQuery(TRequest r)
        => throw new NotImplementedException($"Please override the {nameof(MapToCommandOrQuery)} method!");

    /// <summary>
    /// override this method and place the logic for mapping the request dto to the desired domain entity
    /// </summary>
    /// <param name="r">the request dto to map from</param>
    /// <param name="ct">a cancellation token</param>
    public virtual Task<TCommandOrQuery> MapToCommandOrQueryAsync(TRequest r, CancellationToken ct = default)
        => throw new NotImplementedException($"Please override the {nameof(MapToCommandOrQueryAsync)} method!");

    /// <summary>
    /// override this method and place the logic for mapping a domain entity to a response dto
    /// </summary>
    /// <param name="e">the domain entity to map from</param>
    public virtual TResponse MapFromResultValue(TResultValue e)
        => throw new NotImplementedException($"Please override the {nameof(MapFromResultValue)} method!");

    /// <summary>
    /// override this method and place the logic for mapping a domain entity to a response dto
    /// </summary>
    /// <param name="e">the domain entity to map from</param>
    /// <param name="ct">a cancellation token</param>
    public virtual Task<TResponse> MapFromResultValueAsync(TResultValue e, CancellationToken ct = default)
        => throw new NotImplementedException($"Please override the {nameof(MapFromResultValueAsync)} method!");

}
