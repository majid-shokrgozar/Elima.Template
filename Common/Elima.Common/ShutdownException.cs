using System;
using System.Runtime.Serialization;

namespace Elima.Common;

public class ShutdownException : ElimaException
{
    public ShutdownException()
    {

    }

    public ShutdownException(string message)
        : base(message)
    {

    }

    public ShutdownException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public ShutdownException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
