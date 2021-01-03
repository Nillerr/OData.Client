using System;

namespace OData.Client
{
    public sealed class Optional<TEntity, TValue> :
        IOptional<TEntity, TValue>,
        IEquatable<Optional<TEntity, TValue>>,
        IEquatable<Required<TEntity, TValue>>
        where TEntity : IEntity
        where TValue : notnull
    {
        public Optional(string name) => Name = name;

        public string Name { get; }

        public Type ValueType => typeof(TValue);

        public Type EntityType => typeof(TEntity);

        public static implicit operator Optional<TEntity, TValue>(string str) => new(str);

        public bool Equals(Optional<TEntity, TValue>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public bool Equals(Required<TEntity, TValue>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) => obj switch
        {
            Optional<TEntity, TValue> optional => Equals(optional),
            Required<TEntity, TValue> required => Equals(required),
            _ => false
        };

        public override int GetHashCode() => Name.GetHashCode();

        public static ODataFilter<TEntity> operator ==(Optional<TEntity, TValue> property, IProperty<TEntity, TValue> other) => property.EqualTo(other);
        public static ODataFilter<TEntity> operator !=(Optional<TEntity, TValue> property, IProperty<TEntity, TValue> other) => property.NotEqualTo(other);

        public static ODataFilter<TEntity> operator ==(Optional<TEntity, TValue> property, TValue value) => property.EqualTo(value);
        public static ODataFilter<TEntity> operator !=(Optional<TEntity, TValue> property, TValue value) => property.NotEqualTo(value);

        public static ODataFilter<TEntity> operator >(Optional<TEntity, TValue> property, TValue value) => property.GreaterThan(value);
        public static ODataFilter<TEntity> operator <(Optional<TEntity, TValue> property, TValue value) => property.LessThan(value);

        public static ODataFilter<TEntity> operator >=(Optional<TEntity, TValue> property, TValue value) => property.GreaterThanOrEqualTo(value);
        public static ODataFilter<TEntity> operator <=(Optional<TEntity, TValue> property, TValue value) => property.LessThanOrEqualTo(value);

        public override string ToString() => $"{nameof(Name)}: {Name}";
    }
}