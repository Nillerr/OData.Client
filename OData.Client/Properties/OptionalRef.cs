using System;

namespace OData.Client
{
    /// <inheritdoc cref="IOptionalRef{TEntity,TOther}" />
    public sealed class OptionalRef<TEntity, TOther> :
        IOptionalRef<TEntity, TOther>,
        IEquatable<IRef<TEntity, TOther>>
        where TEntity : IEntity
        where TOther : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalRef{TEntity,TOther}"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        public OptionalRef(string name) => ReferenceName = name;

        /// <inheritdoc />
        public string ReferenceName { get; }

        /// <inheritdoc />
        public string SelectableName => $"_{ReferenceName}_value";

        /// <inheritdoc />
        public string ExpandableName => ReferenceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalRef{TEntity,TOther}"/> class using the string as the
        /// property name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The newly created <see cref="OptionalRef{TEntity,TOther}"/> instance.</returns>
        public static implicit operator OptionalRef<TEntity, TOther>(string name) => new(name);

        /// <inheritdoc />
        public bool Equals(IRef<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SelectableName == other.SelectableName;
        }

        /// <inheritdoc />
        public IProperty<TEntity, IEntityId<TOther>> Value() => new Optional<TEntity, IEntityId<TOther>>(SelectableName);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is IRef<TEntity, TOther> other && Equals(other);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => SelectableName.GetHashCode();
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> references the
        /// <paramref name="other"/> id.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator ==(OptionalRef<TEntity, TOther> property, IEntityId<TOther> other) => 
            property.References(other);

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> does not reference the
        /// <paramref name="other"/> id.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator !=(OptionalRef<TEntity, TOther> property, IEntityId<TOther> other) => 
            property.DoesNotReference(other);

        /// <inheritdoc />
        public override string ToString() => $"{nameof(SelectableName)}: {SelectableName}";
    }
}