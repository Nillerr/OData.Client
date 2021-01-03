using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    public class ODataClientException : Exception
    {
        public ODataClientException()
        {
        }

        protected ODataClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ODataClientException(string? message)
            : base(message)
        {
        }

        public ODataClientException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}