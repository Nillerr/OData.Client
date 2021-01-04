using OData.Client.Expressions;

namespace OData.Client
{
    public static class OptionalRefOperators
    {
        public static Optional<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this OptionalRef<TEntity, TOther> property,
            IProperty<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : notnull
        {
            return $"{property.Name}/{other.Name}";
        }
        
        public static Optional<TEntity, IEntityId<TValue>> Where<TEntity, TOther, TValue>(
            this OptionalRef<TEntity, TOther> property,
            IRef<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : IEntity
        {
            return $"{property.Name}/{other.ValueName()}";
        }

        public static ODataFilter<TEntity> IsNull<TEntity, TOther>(this OptionalRef<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var left = new ODataPropertyExpression(property);
            var right = new ODataConstantExpression(null, typeof(object));
            var expression = new ODataBinaryExpression(left, "eq", right);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> IsNotNull<TEntity, TOther>(this OptionalRef<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var left = new ODataPropertyExpression(property);
            var right = new ODataConstantExpression(null, typeof(object));
            var expression = new ODataBinaryExpression(left, "ne", right);
            return new ODataFilter<TEntity>(expression);
        }
    }
}