namespace OData.Client
{
    public sealed class Field<TEntity, TValue> : IField<TEntity>
        where TEntity : IEntity
    {
        public string Name { get; }

        public Field(string name)
        {
            Name = name;
        }

        public static implicit operator Field<TEntity, TValue>(string str)
        {
            return new(str);
        }

        public static ODataFilter<TEntity> operator ==(Field<TEntity, TValue> field, TValue value)
        {
            return field.EqualTo(value);
        }

        public static ODataFilter<TEntity> operator !=(Field<TEntity, TValue> field, TValue value)
        {
            return field.NotEqualTo(value);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }

        // public static ODataFilter<TEntity> operator >(Field<TEntity, TValue> field, TValue value)
        // {
        //     return field.GreaterThan(value);
        // }
        //
        // public static ODataFilter<TEntity> operator >=(Field<TEntity, TValue> field, TValue value)
        // {
        //     return field.GreaterThanOrEqualTo(value);
        // }
        //
        // public static ODataFilter<TEntity> operator <(Field<TEntity, TValue> field, TValue value)
        // {
        //     return field.LessThan(value);
        // }
        //
        // public static ODataFilter<TEntity> operator <=(Field<TEntity, TValue> field, TValue value)
        // {
        //     return field.LessThanOrEqualTo(value);
        // }

        // TODO @nije: Implement Lambda Operators (https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/query-data-web-api#use-lambda-operators)
    }
}