using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    /// <summary>
    /// A serializable type of <see cref="IProperty"/>.
    /// </summary>
    [Serializable]
    public sealed class SerializableProperty : IProperty, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableProperty"/> class.
        /// </summary>
        /// <param name="property">The property to create a copy of.</param>
        public SerializableProperty(IProperty property)
        {
            Name = property.Name;
            ValueType = property.ValueType;
            EntityType = property.EntityType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableProperty"/> class.
        /// </summary>
        protected SerializableProperty(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetNonNullableString("Name");
            ValueType = info.GetNonNullableType("ValueType");
            EntityType = info.GetNonNullableType("EntityType");
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SortableName => Name;

        /// <inheritdoc />
        public string SelectableName => Name;

        /// <inheritdoc />
        public Type ValueType { get; }

        /// <inheritdoc />
        public Type EntityType { get; }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("ValueType", ValueType.AssemblyQualifiedName);
            info.AddValue("EntityType", ValueType.AssemblyQualifiedName);
        }
    }
}