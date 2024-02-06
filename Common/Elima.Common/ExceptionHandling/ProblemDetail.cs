using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Elima.Common.ExceptionHandling;

public class ProblemDetail
{
    public ProblemDetail()
    {
    }

    public ProblemDetail(
        string message, 
        string? description=null, 
        string? details=null, 
        string? code=null,
        IDictionary? data = null)
    {
        Message = message;
        Description = description;
        Details = details;
        Code = code;
        Data = data;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Code { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary? Data { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Details { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}