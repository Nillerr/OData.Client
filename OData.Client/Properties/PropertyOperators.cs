using OData.Client.Expressions;
using OData.Client.Expressions.Functions;

namespace OData.Client
{
    public static class PropertyOperators
    {
        #region <property> <operator> null

        public static ODataFilter<TEntity> IsNotNull<TEntity, TValue>(this IProperty<TEntity, TValue> property)
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression(property);
            var right = ODataConstantExpression.Null;
            var expression = new ODataBinaryExpression(left, "ne", right);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> IsNull<TEntity, TValue>(this IProperty<TEntity, TValue> property)
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression(property);
            var right = ODataConstantExpression.Null;
            var expression = new ODataBinaryExpression(left, "eq", right);
            return new ODataFilter<TEntity>(expression);
        }
        
        #endregion
        
        #region <property> <operator> <value>

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is equal to the <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
        {
            return Binary(property, "eq", value);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is not equal to the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
        {
            return Binary(property, "ne", value);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> GreaterThan<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
        {
            return Binary(property, "gt", value);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> LessThan<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
        {
            return Binary(property, "lt", value);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than or equal to the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> GreaterThanOrEqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
        {
            return Binary(property, "ge", value);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than or equal to the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> LessThanOrEqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
        {
            return Binary(property, "le", value);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TValue>(
            IProperty<TEntity, TValue> property,
            string @operator,
            TValue value
        )
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression(property);
            var right = ODataConstantExpression.Create(property, value);
            var expression = new ODataBinaryExpression(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }

        #endregion
        
        #region <property> <operator> <property>

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            return Binary(property, "eq", other);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is not equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            return Binary(property, "ne", other);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> GreaterThan<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            return Binary(property, "gt", other);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> LessThan<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            return Binary(property, "lt", other);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is greater than or equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> GreaterThanOrEqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            return Binary(property, "ge", other);
        }

        /// <summary>
        /// Creates a filter that checks whether the <paramref name="property"/> is less than or equal to the
        /// <paramref name="other"/> property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="other">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The filter.</returns>
        public static ODataFilter<TEntity> LessThanOrEqualTo<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            return Binary(property, "le", other);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TValue>(
            IProperty<TEntity, TValue> property,
            string @operator,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression(property);
            var right = new ODataPropertyExpression(other);
            var expression = new ODataBinaryExpression(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }

        #endregion

        #region <function>(<property>,<value>)
        
        public static ODataFilter<TEntity> Contains<TEntity>(this IProperty<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringContainsFunction.Instance, value);
        }

        public static ODataFilter<TEntity> EndsWith<TEntity>(this IProperty<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringEndsWithFunction.Instance, value);
        }

        public static ODataFilter<TEntity> StartsWith<TEntity>(this IProperty<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringStartsWithFunction.Instance, value);
        }

        private static ODataFilter<TEntity> Function<TEntity, TValue>(
            this IProperty<TEntity, TValue> property,
            IODataFunction function,
            TValue value
        )
            where TEntity : IEntity
        {
            var target = new ODataPropertyExpression(property);
            var argument = ODataConstantExpression.Create(property, value);
            var expression = new ODataFunctionExpression(function, target, argument);
            return new ODataFilter<TEntity>(expression);
        }

        #endregion
    }
}