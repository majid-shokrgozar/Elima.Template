using Elima.Common;
using Elima.Common.DependencyInjection;
using Elima.Common.ExceptionHandling;
using Elima.Common.Security.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;

public class DefaultExceptionToErrorInfoConverter : IExceptionToErrorInfoConverter, ITransientDependency
{
    public ProblemDetails Convert(Exception exception, Action<ExceptionHandlingOptions>? options = null)
    {
        var exceptionHandlingOptions = CreateDefaultOptions();
        options?.Invoke(exceptionHandlingOptions);

        ProblemDetails problemDetails = CreateErrorInfoWithoutCode(exception, exceptionHandlingOptions);

        if (exception is IHasErrorCode hasErrorCodeException && problemDetails?.Details?.Count == 1)
            problemDetails.Details[0].Code = hasErrorCodeException.Code;

        return problemDetails!;
    }


    protected virtual ProblemDetails CreateErrorInfoWithoutCode(Exception exception, ExceptionHandlingOptions options)
    {
        if (options.SendExceptionsDetailsToClients)
        {
            return CreateDetailedErrorInfoFromException(exception, options.SendStackTraceToClients);
        }

        exception = TryToGetActualException(exception);

        //if (exception is AbpRemoteCallException remoteCallException && remoteCallException.Error != null)
        //{
        //    return remoteCallException.Error;
        //}

        if (exception is DbConcurrencyException)
        {
            return new ProblemDetails("AbpDbConcurrencyErrorMessage");
        }

        //if (exception is EntityNotFoundException)
        //{
        //    return CreateEntityNotFoundError((exception as EntityNotFoundException)!);
        //}

        var problemDetails = new ProblemDetails();
        var errorInfo = new ProblemDetail();

        if (exception is IUserFriendlyException)
        {
            errorInfo.Message = exception.Message;
            errorInfo.Details = (exception as IHasErrorDetails)?.Details;
        }

        if (exception is IHasValidationErrors)
        {
            if (errorInfo.Message.IsNullOrEmpty())
            {
                errorInfo.Message = "ValidationErrorMessage";
            }

            if (errorInfo.Details.IsNullOrEmpty())
            {
                errorInfo.Details = GetValidationErrorNarrative((exception as IHasValidationErrors)!);
            }

            problemDetails.ValidationErrors = GetValidationErrorInfos((exception as IHasValidationErrors)!);
        }

        TryToLocalizeExceptionMessage(exception, errorInfo);

        if (errorInfo.Message.IsNullOrEmpty())
        {
            errorInfo.Message = "InternalServerErrorMessage";
        }

        errorInfo.Data = exception.Data;

        problemDetails.Details = [errorInfo];

        return problemDetails;
    }

    protected virtual void TryToLocalizeExceptionMessage(Exception exception, ProblemDetail errorInfo)
    {
        //if (exception is ILocalizeErrorMessage localizeErrorMessageException)
        //{
        //    using (var scope = ServiceProvider.CreateScope())
        //    {
        //        errorInfo.Message = localizeErrorMessageException.LocalizeMessage(new LocalizationContext(scope.ServiceProvider));
        //    }

        //    return;
        //}

        //if (!(exception is IHasErrorCode exceptionWithErrorCode))
        //{
        //    return;
        //}

        //if (exceptionWithErrorCode.Code.IsNullOrWhiteSpace() ||
        //    !exceptionWithErrorCode.Code!.Contains(":"))
        //{
        //    return;
        //}

        //var codeNamespace = exceptionWithErrorCode.Code.Split(':')[0];

        //var localizationResourceType = LocalizationOptions.ErrorCodeNamespaceMappings.GetOrDefault(codeNamespace);
        //if (localizationResourceType == null)
        //{
        //    return;
        //}

        //var stringLocalizer = StringLocalizerFactory.Create(localizationResourceType);
        //var localizedString = stringLocalizer[exceptionWithErrorCode.Code];
        //if (localizedString.ResourceNotFound)
        //{
        //    return;
        //}

        //var localizedValue = localizedString.Value;

        //if (exception.Data != null && exception.Data.Count > 0)
        //{
        //    foreach (var key in exception.Data.Keys)
        //    {
        //        localizedValue = localizedValue.Replace("{" + key + "}", exception.Data[key]?.ToString());
        //    }
        //}

        //errorInfo.Message = localizedValue;
    }

    protected virtual ProblemDetails CreateDetailedErrorInfoFromException(Exception exception, bool sendStackTraceToClients)
    {
        var problems = new List<ProblemDetail>();

        AddExceptionToDetails(exception,ref problems, sendStackTraceToClients);

        var problemDetails = new ProblemDetails
        {
            Details = problems
        };

        if (exception is ValidationException)
        {
            problemDetails.Title = "Validation Error";
            problemDetails.ValidationErrors = GetValidationErrorInfos((exception as ValidationException)!);
        }

        if (string.IsNullOrEmpty(problemDetails.Title))
        {
            problemDetails.Title = problems.Count == 1 ? problems[0].Message : "InternalServerErrorMessage";
        }

        return problemDetails;
    }

    protected virtual Exception TryToGetActualException(Exception exception)
    {
        if (exception is not AggregateException aggException || aggException.InnerException == null)
        {
            return exception;
        }

        if (aggException.InnerException is ValidationException ||
            aggException.InnerException is AuthorizationException ||
            //aggException.InnerException is EntityNotFoundException ||
            aggException.InnerException is IBusinessException)
        {
            return aggException.InnerException;
        }

        return exception;
    }

    protected virtual List<ValidationErrorInfo> GetValidationErrorInfos(IHasValidationErrors validationException)
    {
        var validationErrorInfos = new List<ValidationErrorInfo>();

        foreach (var validationResult in validationException.ValidationErrors)
        {
            var validationError = new ValidationErrorInfo(validationResult.ErrorMessage!);

            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                validationError.Members = validationResult.MemberNames.Select(m => m.ToCamelCase()).ToArray();
            }

            validationErrorInfos.Add(validationError);
        }

        return validationErrorInfos;
    }

    protected virtual void AddExceptionToDetails(Exception exception, ref List<ProblemDetail> problems, bool sendStackTraceToClients)
    {
        var problemDetail = new ProblemDetail();
        var detailBuilder = new StringBuilder();
        //Exception Message

        problemDetail.Message = exception.Message;


        detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

        //Additional info for UserFriendlyException
        if (exception is IUserFriendlyException &&
            exception is IHasErrorDetails)
        {
            var details = ((IHasErrorDetails)exception).Details;
            if (!string.IsNullOrEmpty(details))
            {
                detailBuilder.AppendLine(details);

            }
        }

        //Additional info for AbpValidationException
        if (exception is ValidationException validationException && validationException.ValidationErrors.Count > 0)
        {
            detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
        }

        problemDetail.Details = detailBuilder.ToString();

        //Exception StackTrace
        if (sendStackTraceToClients && !string.IsNullOrEmpty(exception.StackTrace))
        {
            problemDetail.StackTrace = exception.StackTrace;
        }

        problems.Add(problemDetail);

        //Inner exception
        if (exception.InnerException != null)
        {
            AddExceptionToDetails(exception.InnerException,ref problems, sendStackTraceToClients);
        }

        //Inner exceptions for AggregateException
        if (exception is AggregateException aggException)
        {
            if (aggException.InnerExceptions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var innerException in aggException.InnerExceptions)
            {
                AddExceptionToDetails(innerException,ref problems, sendStackTraceToClients);
            }
        }
    }

    protected virtual string GetValidationErrorNarrative(IHasValidationErrors validationException)
    {
        var detailBuilder = new StringBuilder();

        //TODO Localization
        // L["ValidationNarrativeErrorMessageTitle"]
        detailBuilder.AppendLine("ValidationNarrativeErrorMessageTitle");

        foreach (var validationResult in validationException.ValidationErrors)
        {
            detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
            detailBuilder.AppendLine();
        }

        return detailBuilder.ToString();
    }

    protected virtual ExceptionHandlingOptions CreateDefaultOptions()
    {
        return new ExceptionHandlingOptions
        {
            SendExceptionsDetailsToClients = false,
            SendStackTraceToClients = true
        };
    }

}
