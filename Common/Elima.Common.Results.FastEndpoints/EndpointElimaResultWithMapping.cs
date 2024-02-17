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


public abstract class EndpointElimaResultWithMapping<TRequest, TCommandOrQuery>:Endpoint<TRequest>
    where TRequest : notnull
{
    public Task SendResultAsync(IResult result, CancellationToken cancellationToken)
    {
        return result.Status switch
        {
            ResultStatus.Succeeded => SendSucceededAsync(cancellationToken),
            ResultStatus.BadRequest => SendBadRequestAsync(result, cancellationToken),
            ResultStatus.Unauthorized => SendUnauthorizedAsync(result, cancellationToken),
            ResultStatus.Forbidden => SendForbiddenAsync(result, cancellationToken),
            ResultStatus.NotFound => SendNotFoundAsync(result, cancellationToken),
            ResultStatus.Conflict => SendConflictAsync(result, cancellationToken),
            ResultStatus.Failed => SendFailedAsync(result, cancellationToken),
            ResultStatus.NotImplemented => SendNotImplementedAsync(result, cancellationToken),
            ResultStatus.Unprocessable => SendNotProcessableAsync(result, cancellationToken),
            _ => throw new NotImplementedException(),
        };
    }


    protected async Task SendNotProcessableAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendNotImplementedAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendFailedAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendConflictAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendNotFoundAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendForbiddenAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendUnauthorizedAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, string.Empty, string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendBadRequestAsync(IResult result, CancellationToken cancellationToken)
    {
        await SendProblemDetailsAsync(ConvertResultToProblemDetails(result, "ValidationError", string.Empty, string.Empty), (int)result.Status, cancellationToken);
    }

    protected async Task SendProblemDetailsAsync(Elima.Common.ExceptionHandling.ProblemDetails problemDetails, int httpStatus, CancellationToken cancellationToken)
    {
        await SendStringAsync(System.Text.Json.JsonSerializer.Serialize(problemDetails), httpStatus, "application/json", cancellationToken);
    }

    protected Elima.Common.ExceptionHandling.ProblemDetails ConvertResultToProblemDetails(IResult result, string defaultError, string instance, string type)
    {
        var problemDetails = new Elima.Common.ExceptionHandling.ProblemDetails
        {
            Title = result.Errors.Count == 1 ? result.Errors[0].Message : defaultError,
            Status = (int)result.Status,
            Type = type,
            Instance = instance
        };

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
                Code = error.ErrorCode,
                AttemptedValue = error.AttemptedValue
            });
        }

        return problemDetails;
    }

    private async Task SendSucceededAsync(CancellationToken cancellationToken)
    {
        await SendNoContentAsync(cancellationToken);
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
        => Task.FromResult(MapToCommandOrQuery(r));
}

public abstract class EndpointElimaResultWithMapping<TRequest, TResponse, TCommandOrQuery, TResultValue> : EndpointElimaResultWithMapping<TRequest, TCommandOrQuery>
    where TRequest : notnull
{

    public Task SendResultAsync(IResultWithValue<TResultValue> result, Func<TResultValue, TResponse>? mapper, CancellationToken cancellationToken)
    {
        if (result.Status == ResultStatus.Succeeded)
            return SendSucceededAsync(result, mapper, cancellationToken);

        return base.SendResultAsync(result, cancellationToken);
    }

    public Task SendResultAsync(IResultWithValue<TResultValue> result, CancellationToken cancellationToken)
    {
        return SendResultAsync(result, null, cancellationToken);
    }

    protected async Task SendSucceededAsync(IResultWithValue<TResultValue> result, Func<TResultValue, TResponse>? mapper, CancellationToken cancellationToken)
    {
        if (result.Value is null)
        {
            await SendEmptyJsonObject(cancellationToken);
            return;
        }
        TResponse? response;

        if (mapper != null)
        {
            response = mapper.Invoke(result.Value);
        }
        else
        {
            response = await MapToResponseAsync(result.Value, cancellationToken);
        }

        await SendStringAsync(System.Text.Json.JsonSerializer.Serialize(response), 200, "application/json", cancellationToken);
    }

    public virtual TResponse MapToResponse(TResultValue e)
        => throw new NotImplementedException($"Please override the {nameof(MapToResponseAsync)} method!");

    public virtual Task<TResponse> MapToResponseAsync(TResultValue e, CancellationToken ct = default)
        => Task.FromResult(MapToResponse(e));

}
