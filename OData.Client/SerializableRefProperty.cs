using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    /// <summary>
    /// A serializable type of <see cref="IRefProperty"/>.
    /// </summary>
    [Serializable]
    public sealed class SerializableRefProperty : IRefProperty, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableRefProperty"/> class.
        /// </summary>
        /// <param name="property">The property to create a copy of.</param>
        public SerializableRefProperty(IRefProperty property)
        {
            ReferenceName = property.ReferenceName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableRefProperty"/> class.
        /// </summary>
        protected SerializableRefProperty(SerializationInfo info, StreamingContext context)
        {
            ReferenceName = info.GetNonNullableString("ReferenceName");
        }

        /// <inheritdoc />
        public string ReferenceName { get; }

        /// <inheritdoc />
        public string SelectableName => $"_{ReferenceName}_value";

        public string ExpandableName => ReferenceName;

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ReferenceName", ReferenceName);
        }
    }
}