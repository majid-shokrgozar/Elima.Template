using System;
using System.Collections.Generic;

namespace Elima.Common.Results;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(DefaulErroCodes.NullValue, "The error value is null");
    public static readonly Error Unauthorized = new(DefaulErroCodes.Unauthorized, "full authentication is required to access this resource");
    public static readonly Error Forbidden = new(DefaulErroCodes.Forbidden, "you don't have permission to access this resource");
    public static readonly Error NotImplemented = new(DefaulErroCodes.NotImplemented, "Requested method or operation is not implemented.");
    public static readonly Error Conflict = new(DefaulErroCodes.Conflict, "Data conflicts between the request and the server, which cannot be successfully completed");
    public static Error NotFound(Dictionary<string, object> data) => new("Error.NotFound", "The entity not found", data);

    public Error(string code, string message, Dictionary<string, object>? data = null, Exception? exception = null)
    {
        Code = code;
        Message = message;
        Data = data;
        Exception = exception;
    }

    public Error(string code, string message) : this(code, message, null, null)
    {
    }

    public Error(string code, string message, Exception exception) : this(code, message, null, exception)
    {
    }

    public Error(string code, string message, Dictionary<string, object> data) : this(code, message, data, null)
    {
    }


    public string Code { get; set; }
    public string Message { get; set; }
    public Dictionary<string, object>? Data { get; set; }
    public Exception? Exception { get; set; }

    public static implicit operator string(Error error) => error.Code;


    public bool Equals(Error? other)
    {
        if (other == null) return false;

        if (GetType() != other.GetType()) return false;

        return Code.Equals(other.Code, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (GetType() != obj.GetType()) return false;

        if (obj is not Error error)
        {
            return false;
        }
        return Code.Equals(error.Code, StringComparison.InvariantCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode() + 35;
    }
}