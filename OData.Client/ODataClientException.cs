using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    /// <summary>
    /// The exception thrown when an operation in <see cref="ODataClient"/> results in an error.
    /// </summary>
    [Serializable]
    public class ODataClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataClientException"/> class.
        /// </summary>
        public ODataClientException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataClientException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ODataClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ODataClientException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified</param>
        public ODataClientException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}