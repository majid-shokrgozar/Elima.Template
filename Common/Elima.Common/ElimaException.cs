using System;
using System.Runtime.Serialization;

namespace Elima.Common;

/// <summary>
/// Base exception type for those are thrown by Abp system for Abp specific exceptions.
/// </summary>
public class ElimaException : Exception
{
    public ElimaException()
    {

    }

    public ElimaException(string? message)
        : base(message)
    {

    }

    public ElimaException(string? message, Exception? innerException)
        : base(message, innerException)
    {

    }

    public ElimaException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
