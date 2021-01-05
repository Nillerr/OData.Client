using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    /// <summary>
    /// The exception thrown when the value of an entity property was unexpectedly <see langword="null"/>.
    /// </summary>
    [Serializable]
    public class ODataNullRefException : ODataClientException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataNullValueException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="property">The entity property containing the <see langword="null"/> value.</param>
        public ODataNullRefException(string? message, IRefProperty property)
            : base(message)
        {
            Property = property;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataNullValueException"/>.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ODataNullRefException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Property = info.GetNonNullableRefProperty("Property");
        }

        /// <summary>
        /// The entity property containing the <see langword="null"/> value.
        /// </summary>
        public IRefProperty Property { get; }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddRefProperty("Property", Property);
            
            base.GetObjectData(info, context);
        }
    }
}