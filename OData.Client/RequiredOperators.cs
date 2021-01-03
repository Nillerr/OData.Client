using OData.Client.Expressions;
using OData.Client.Expressions.Functions;

namespace OData.Client
{
    public static class RequiredOperators
    {
        #region <property> <operator> <value>
        
        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "eq", value);
        }

        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "ne", value);
        }

        public static ODataFilter<TEntity> GreaterThan<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "gt", value);
        }

        public static ODataFilter<TEntity> LessThan<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "lt", value);
        }

        public static ODataFilter<TEntity> GreaterThanOrEqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "ge", value);
        }

        public static ODataFilter<TEntity> LessThanOrEqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "le", value);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TValue>(
            IRequired<TEntity, TValue> property,
            string @operator,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            var left = new ODataPropertyExpression(property);
            var right = new ODataConstantExpression(value, typeof(TValue));
            var expression = new ODataBinaryExpression(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }
        
        #endregion

        #region <property> <operator> <property>

        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "eq", other);
        }

        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "ne", other);
        }

        public static ODataFilter<TEntity> GreaterThan<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "gt", other);
        }

        public static ODataFilter<TEntity> LessThan<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "lt", other);
        }

        public static ODataFilter<TEntity> GreaterThanOrEqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "ge", other);
        }

        public static ODataFilter<TEntity> LessThanOrEqualTo<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "le", other);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TValue>(
            IRequired<TEntity, TValue> property,
            string @operator,
            IProperty<TEntity, TValue> other
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            var left = new ODataPropertyExpression(property);
            var right = new ODataPropertyExpression(other);
            var expression = new ODataBinaryExpression(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }

        #endregion

        #region <function>(<property>,<value>)

        public static ODataFilter<TEntity> Contains<TEntity>(this IRequired<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringContainsFunction.Instance, value);
        }

        public static ODataFilter<TEntity> EndsWith<TEntity>(this IRequired<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringEndsWithFunction.Instance, value);
        }

        public static ODataFilter<TEntity> StartsWith<TEntity>(this IRequired<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringStartsWithFunction.Instance, value);
        }

        private static ODataFilter<TEntity> Function<TEntity, TValue>(
            this IRequired<TEntity, TValue> property,
            IODataFunction function,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            var target = new ODataPropertyExpression(property);
            var argument = new ODataConstantExpression(value, typeof(TValue));
            var expression = new ODataFunctionExpression(function, target, argument);
            return new ODataFilter<TEntity>(expression);
        }

        #endregion
    }
}