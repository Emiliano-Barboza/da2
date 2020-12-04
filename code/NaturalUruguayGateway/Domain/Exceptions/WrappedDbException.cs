using System;
using System.Runtime.Serialization;

namespace NaturalUruguayGateway.Domain.Exceptions
{
    public class WrappedDbException : Exception
    {
        public WrappedDbException() : base() { }
        public WrappedDbException(string message) : base(message) { }
        public WrappedDbException(string message, Exception ex) : base(message, ex) { }
        protected WrappedDbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}