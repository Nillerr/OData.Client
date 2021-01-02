using System;

namespace OData.Client
{
    public sealed class Property<TEntity, TValue> : IProperty<TEntity>
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