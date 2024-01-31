using System.Runtime.Serialization;
using System;

namespace Elima.Common.EntityFramework.EntityFrameworkCore
{
    public class DbConcurrencyException:ElimaException
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

        public DbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
