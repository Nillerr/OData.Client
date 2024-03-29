using System;

namespace OData.Client
{
    /// <inheritdoc cref="IProperty{TEntity,TValue}"/>
    public sealed class Property<TEntity, TValue> : IProperty<TEntity, TValue>, IEquatable<IProperty<TEntity, TValue>>
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property{TEntity,TValue}"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        public Property(string name) => Name = name;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SelectableName => Name;

        /// <inheritdoc />
        public string SortableName => Name;

        /// <inheritdoc />
        public Type ValueType => typeof(TValue);

        /// <inheritdoc />
        public Type EntityType => typeof(TEntity);

        /// <summary>
        /// Initializes a new instance of the <see cref="Property{TEntity,TValue}"/> class using the string as the
        /// property name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The newly created <see cref="Property{TEntity,TValue}"/> instance.</returns>
        public static implicit operator Property<TEntity, TValue>(string name) => new(name);

        /// <inheritdoc />
        public bool Equals(IProperty<TEntity, TValue>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is IProperty<TEntity, TValue> other && Equals(other);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => Name.GetHashCode();

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator ==(Property<TEntity, TValue> property, IProperty<TEntity, TValue> other) => 
            property.EqualTo(other);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is not equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator !=(Property<TEntity, TValue> property, IProperty<TEntity, TValue> other) => 
            property.NotEqualTo(other);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator >(Property<TEntity, TValue> property, IProperty<TEntity, TValue> other) => 
            property.GreaterThan(other);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator <(Property<TEntity, TValue> property, IProperty<TEntity, TValue> other) => 
            property.LessThan(other);

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than or equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator >=(Property<TEntity, TValue> property, IProperty<TEntity, TValue> other) => 
            property.GreaterThanOrEqualTo(other);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than or equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator <=(Property<TEntity, TValue> property, IProperty<TEntity, TValue> other) => 
            property.LessThanOrEqualTo(other);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is equal to the <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator ==(Property<TEntity, TValue> property, TValue value) => 
            property.EqualTo(value);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is not equal to the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator !=(Property<TEntity, TValue> property, TValue value) => 
            property.NotEqualTo(value);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator >(Property<TEntity, TValue> property, TValue value) => 
            property.GreaterThan(value);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator <(Property<TEntity, TValue> property, TValue value) => 
            property.LessThan(value);

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than or equal to the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator >=(Property<TEntity, TValue> property, TValue value) => 
            property.GreaterThanOrEqualTo(value);
        
        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than or equal to the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> operator <=(Property<TEntity, TValue> property, TValue value) => 
            property.LessThanOrEqualTo(value);

        /// <inheritdoc />
        public override string ToString() => $"{nameof(Name)}: {Name}";
    }
}