using Elima.Common.ExceptionHandling;
using Elima.Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Text;

namespace Elima.Common.Security.Authentication;

[Serializable]
public class AuthorizationException : ElimaException, IHasLogLevel, IHasErrorCode
{
    public LogLevel LogLevel { get; set; }

    public string? Code { get; }

    public AuthorizationException()
    {
        LogLevel = LogLevel.Warning;
    }

    protected AuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }

    public AuthorizationException(string message)
        : base(message)
    {
        LogLevel = LogLevel.Warning;
    }

    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException)
    {
        LogLevel = LogLevel.Warning;
    }

    public AuthorizationException(string? message, string? code, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        LogLevel = LogLevel.Warning;
    }

    public AuthorizationException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}