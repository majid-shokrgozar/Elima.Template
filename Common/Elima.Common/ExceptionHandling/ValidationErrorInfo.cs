using System.Text.Json.Serialization;

namespace Elima.Common.ExceptionHandling;

public class ValidationErrorInfo
{

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[] Members { get; set; } = default!;


    public ValidationErrorInfo()
    {

    }


    public ValidationErrorInfo(string message)
    {
        Message = message;
    }


    public ValidationErrorInfo(string message, string[] members)
        : this(message)
    {
        Members = members;
    }
    public ValidationErrorInfo(string message, string member)
        : this(message, new[] { member })
    {

    }

}