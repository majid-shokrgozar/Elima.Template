using System;
using System.Runtime.Serialization;

namespace Elima.Common;

public class InitializationException : ElimaException
{
    public InitializationException()
    {

    }

    public InitializationException(string message)
        : base(message)
    {

    }

    public InitializationException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public InitializationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
