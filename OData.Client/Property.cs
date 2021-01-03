using System;

namespace OData.Client
{
    public sealed class Property<TEntity, TValue> : IProperty<TEntity, TValue>, IEquatable<Property<TEntity, TValue>>
        where TEntity : IEntity
    {
        public Property(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Type ValueType => typeof(TValue);

        public static implicit operator Property<TEntity, TValue>(string str)
        {
            return new(str);
        }

        public bool Equals(Property<TEntity, TValue>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is Property<TEntity, TValue> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Property<TEntity, TValue>? property, Property<TEntity, TValue>? other)
        {
            return Equals(property, other);
        }

        public static bool operator !=(Property<TEntity, TValue>? property, Property<TEntity, TValue>? other)
        {
            return !Equals(property, other);
        }

        public static ODataFilter<TEntity> operator ==(Property<TEntity, TValue> property, TValue value)
        {
            return property.EqualTo(value);
        }

        public static ODataFilter<TEntity> operator !=(Property<TEntity, TValue> property, TValue value)
        {
            return property.NotEqualTo(value);
        }

        public static ODataFilter<TEntity> operator >(Property<TEntity, TValue> property, TValue value)
        {
            return property.GreaterThan(value);
        }

        public static ODataFilter<TEntity> operator <(Property<TEntity, TValue> property, TValue value)
        {
            return property.LessThan(value);
        }

        public static ODataFilter<TEntity> operator >=(Property<TEntity, TValue> property, TValue value)
        {
            return property.GreaterThanOrEqualTo(value);
        }

        public static ODataFilter<TEntity> operator <=(Property<TEntity, TValue> property, TValue value)
        {
            return property.LessThanOrEqualTo(value);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}