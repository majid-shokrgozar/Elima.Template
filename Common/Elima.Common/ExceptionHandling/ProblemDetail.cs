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
        string? details=null, 
        string? code=null,
        string? stackTrace = null, 
        IDictionary? data = null)
    {
        Message = message;
        Details = details;
        Code = code;
        StackTrace = stackTrace;
        Data = data;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(1)]
    public string? Code { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(5)]
    public IDictionary? Data { get; set; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(4)]
    public string? StackTrace { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(3)]
    public string? Details { get; set; } = string.Empty;

    [JsonPropertyOrder(2)]
    public string Message { get; set; } = string.Empty;
}