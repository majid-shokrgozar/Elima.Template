﻿using Elima.Common.ExceptionHandling;
using Elima.Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Elima.Common;

[Serializable]
public class BusinessException : Exception,
    IBusinessException,
    IHasErrorCode,
    IHasErrorDetails,
    IHasLogLevel
{
    public string? Code { get; set; }

    public string? Details { get; set; }

    public LogLevel LogLevel { get; set; }

    public BusinessException(
        string? code = null,
        string? message = null,
        string? details = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning)
        : base(message, innerException)
    {
        Code = code;
        Details = details;
        LogLevel = logLevel;
    }

    /// <summary>
    /// Constructor for serializing.
    /// </summary>
    public BusinessException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }

    public BusinessException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
