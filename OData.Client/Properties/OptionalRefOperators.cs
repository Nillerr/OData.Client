using System;
using OData.Client.Expressions;

namespace OData.Client
{
    public static class OptionalRefOperators
    {
        public static Property<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this OptionalRef<TEntity, TOther> property,
            IProperty<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return $"{property.Name}/{other.Name}";
        }
        
        public static RequiredRef<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this OptionalRef<TEntity, TOther> property,
            IRequiredRef<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : IEntity
        {
            return RequiredRef<TEntity, TValue>.Prefixed($"{property.Name}/", other.Name);
        }

        public static OptionalRef<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this OptionalRef<TEntity, TOther> property,
            IOptionalRef<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : IEntity
        {
            return OptionalRef<TEntity, TValue>.Prefixed($"{property.Name}/", other.Name);
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
            return BinaryNull(property, "eq");
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
            return BinaryNull(property, "ne");
        }

        private static ODataFilter<TEntity> BinaryNull<TEntity, TOther>(IOptionalRef<TEntity, TOther> property, string @operator)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var valueProperty = new Property<TEntity, Guid>(property.ValueName);
            var left = new ODataPropertyExpression(valueProperty);
            var right = ODataConstantExpression.Null;
            var expression = new ODataBinaryExpression(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }
    }
}