using System;

namespace OData.Client
{
    /// <inheritdoc cref="IRequiredRef{TEntity,TOther}" />
    public sealed class RequiredRef<TEntity, TOther> :
        IRequiredRef<TEntity, TOther>,
        IEquatable<IRef<TEntity, TOther>>
        where TEntity : IEntity
        where TOther : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredRef{TEntity,TOther}"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="valueName">The property value name.</param>
        public RequiredRef(string name, string valueName)
        {
            Name = name;
            ValueName = valueName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredRef{TEntity,TOther}"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        public RequiredRef(string name) : this(name, $"_{name}_value")
        {
        }

        public static RequiredRef<TEntity, TOther> Prefixed(string prefix, string name)
        {
            return new RequiredRef<TEntity, TOther>(prefix + name, $"{prefix}_{name}_value");
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string ValueName { get; }

        /// <inheritdoc />
        public string SelectableName => ValueName;

        /// <inheritdoc />
        public string ExpandableName => Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredRef{TEntity,TOther}"/> class using the string as the
        /// property name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The newly created <see cref="RequiredRef{TEntity,TOther}"/> instance.</returns>
        public static implicit operator RequiredRef<TEntity, TOther>(string name) => new(name);

        /// <inheritdoc />
        public bool Equals(IRef<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && ValueName == other.ValueName;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is IRef<TEntity, TOther> other && Equals(other);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => HashCode.Combine(Name, SelectableName);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> references the
        /// <paramref name="other"/> id.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator ==(RequiredRef<TEntity, TOther> property, IEntityId<TOther> other) => 
            property.References(other);

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> does not reference the
        /// <paramref name="other"/> id.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator !=(RequiredRef<TEntity, TOther> property, IEntityId<TOther> other) => 
            property.DoesNotReference(other);

        /// <inheritdoc />
        public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(ValueName)}: {ValueName}";
    }
}