namespace Elima.Common.Results;

public class ValidationError
{
    public ValidationError(string propertyName, string errorMessage) : this(propertyName, errorMessage, null)
    {

    }

    public ValidationError(string propertyName, string errorMessage, object? attemptedValue) : this(propertyName, errorMessage, attemptedValue, null)
    {

    }

    public ValidationError(string propertyName, string errorMessage, object? attemptedValue, string? errorCode)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
        AttemptedValue = attemptedValue;
        ErrorCode = errorCode;
    }

    public string PropertyName { get; }

    public string ErrorMessage { get; }

    public object? AttemptedValue { get; }

    public string? ErrorCode { get; }

}
