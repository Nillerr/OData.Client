using System;

namespace OData.Client
{
    public sealed class Required<TEntity, TValue> :
        IRequired<TEntity, TValue>,
        IEquatable<Required<TEntity, TValue>>,
        IEquatable<Optional<TEntity, TValue>>
        where TEntity : IEntity
        where TValue : notnull
    {
        public Required(string name) => Name = name;

        public string Name { get; }

        public Type ValueType => typeof(TValue);

        public Type EntityType => typeof(TEntity);

        public static implicit operator Required<TEntity, TValue>(string str) => new(str);

        public bool Equals(Required<TEntity, TValue>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public bool Equals(Optional<TEntity, TValue>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) => obj switch
        {
            Required<TEntity, TValue> required => Equals(required),
            Optional<TEntity, TValue> optional => Equals(optional),
            _ => false
        };

        public override int GetHashCode() => Name.GetHashCode();

        public static bool operator ==(Required<TEntity, TValue>? property, Required<TEntity, TValue>? other) => Equals(property, other);
        public static bool operator !=(Required<TEntity, TValue>? property, Required<TEntity, TValue>? other) => !Equals(property, other);

        // ReSharper disable once SuspiciousTypeConversion.Global
        public static bool operator ==(Required<TEntity, TValue>? property, Optional<TEntity, TValue>? other) => Equals(property, other);
        // ReSharper disable once SuspiciousTypeConversion.Global
        public static bool operator !=(Required<TEntity, TValue>? property, Optional<TEntity, TValue>? other) => !Equals(property, other);

        public static ODataFilter<TEntity> operator ==(Required<TEntity, TValue> property, TValue value) => property.EqualTo(value);
        public static ODataFilter<TEntity> operator !=(Required<TEntity, TValue> property, TValue value) => property.NotEqualTo(value);

        public static ODataFilter<TEntity> operator >(Required<TEntity, TValue> property, TValue value) => property.GreaterThan(value);
        public static ODataFilter<TEntity> operator <(Required<TEntity, TValue> property, TValue value) => property.LessThan(value);

        public static ODataFilter<TEntity> operator >=(Required<TEntity, TValue> property, TValue value) => property.GreaterThanOrEqualTo(value);
        public static ODataFilter<TEntity> operator <=(Required<TEntity, TValue> property, TValue value) => property.LessThanOrEqualTo(value);

        public override string ToString() => $"{nameof(Name)}: {Name}";
    }
}