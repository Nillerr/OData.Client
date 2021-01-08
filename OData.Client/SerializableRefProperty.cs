using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    /// <summary>
    /// A serializable type of <see cref="IRefProperty"/>.
    /// </summary>
    [Serializable]
    public class SerializableRefProperty : IRefProperty, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableRefProperty"/> class.
        /// </summary>
        /// <param name="property">The property to create a copy of.</param>
        public SerializableRefProperty(IRefProperty property)
        {
            Name = property.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableRefProperty"/> class.
        /// </summary>
        protected SerializableRefProperty(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetNonNullableString("ReferenceName");
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SelectableName => $"_{Name}_value";

        public string ExpandableName => Name;

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ReferenceName", Name);
        }
    }
}