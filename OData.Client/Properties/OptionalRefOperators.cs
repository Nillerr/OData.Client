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
            return $"{property.SelectableName}/{other.SelectableName}";
        }
        
        public static Optional<TEntity, IEntityId<TValue>> Where<TEntity, TOther, TValue>(
            this OptionalRef<TEntity, TOther> property,
            IRef<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : IEntity
        {
            return $"{property.SelectableName}/{other.ValueName()}";
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> has no reference.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> HasNoReference<TEntity, TOther>(this IOptionalRef<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var valueProperty = property.Value();
            var left = new ODataPropertyExpression(valueProperty);
            var right = new ODataConstantExpression(null, typeof(object));
            var expression = new ODataBinaryExpression(left, "eq", right);
            return new ODataFilter<TEntity>(expression);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> has a reference to something.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> HasReference<TEntity, TOther>(this IOptionalRef<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var valueProperty = property.Value();
            var left = new ODataPropertyExpression(valueProperty);
            var right = new ODataConstantExpression(null, typeof(object));
            var expression = new ODataBinaryExpression(left, "ne", right);
            return new ODataFilter<TEntity>(expression);
        }
    }
}