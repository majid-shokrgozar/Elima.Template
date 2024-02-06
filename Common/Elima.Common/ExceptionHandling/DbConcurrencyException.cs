using System.Runtime.Serialization;
using System;

namespace Elima.Common.ExceptionHandling
{
    [Serializable]
    public class DbConcurrencyException : ElimaException
    {
        public DbConcurrencyException()
        {

        }

        public DbConcurrencyException(string? message)
            : base(message)
        {

        }

        public DbConcurrencyException(string? message, Exception? innerException)
            : base(message, innerException)
        {

        }

        protected DbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
