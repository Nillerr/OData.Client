using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    /// <summary>
    /// The exception thrown when the value of an entity property was unexpectedly <see langword="null"/>.
    /// </summary>
    [Serializable]
    public class ODataNullValueException : ODataClientException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataNullValueException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="property">The entity property containing the <see langword="null"/> value.</param>
        public ODataNullValueException(string? message, IProperty property)
            : base(message)
        {
            Property = property;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataNullValueException"/>.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ODataNullValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Property = info.GetNonNullableProperty("Property");
        }

        /// <summary>
        /// The entity property containing the <see langword="null"/> value.
        /// </summary>
        public IProperty Property { get; }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddProperty("Property", Property);
            
            base.GetObjectData(info, context);
        }
    }
}