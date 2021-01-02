using System;
using System.Collections.Generic;
using OData.Client.Expressions;
using OData.Client.Expressions.Functions;

namespace OData.Client
{
    public static class PropertyOperators
    {
        public static Property<TEntity, TValue> Filter<TEntity, TOther, TValue>(
            this Property<TEntity, TOther?> property,
            Property<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return $"{property.Name}/{other.Name}";
        }

        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(this Property<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return Binary(property, "eq", value);
        }

        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(this Property<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return Binary(property, "ne", value);
        }

        public static ODataFilter<TEntity> GreaterThan<TEntity, TValue>(this Property<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return Binary(property, "gt", value);
        }

        public static ODataFilter<TEntity> LessThan<TEntity, TValue>(this Property<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return Binary(property, "lt", value);
        }

        public static ODataFilter<TEntity> GreaterThanOrEqualTo<TEntity, TValue>(this Property<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return Binary(property, "ge", value);
        }

        public static ODataFilter<TEntity> LessThanOrEqualTo<TEntity, TValue>(this Property<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return Binary(property, "le", value);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TValue>(Property<TEntity, TValue> property, string @operator, TValue value)
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression<TEntity>(property);
            var right = new ODataConstantExpression<TEntity>(value);
            var expression = new ODataBinaryExpression<TEntity>(left, @operator, right);
            return new ODataFilter<TEntity>(expression);
        }

#nullable disable
        public static ODataFilter<TEntity> Contains<TEntity>(this Property<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringContainsFunction<TEntity>.Instance, value);
        }

        public static ODataFilter<TEntity> EndsWith<TEntity>(this Property<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringEndsWithFunction<TEntity>.Instance, value);
        }

        public static ODataFilter<TEntity> StartsWith<TEntity>(this Property<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringStartsWithFunction<TEntity>.Instance, value);
        }
        #nullable restore

        private static ODataFilter<TEntity> Function<TEntity, TValue>(
            this Property<TEntity, TValue> property,
            IODataFunction<TEntity> function,
            TValue value
        )
            where TEntity : IEntity
        {
            var target = new ODataPropertyExpression<TEntity>(property);
            var argument = new ODataConstantExpression<TEntity>(value);
            var expression = new ODataFunctionExpression<TEntity>(function, target, argument);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> Any<TEntity, TOther, TOthers>(
            this Property<TEntity, TOthers> property,
            ODataFilter<TOther> filter
        )
            where TEntity : IEntity
            where TOthers : IEnumerable<TOther>
            where TOther : IEntity
        {
            var body = CheckLambdaBody(filter, nameof(filter));
            return Lambda(property, "any", body);
        }

        public static ODataFilter<TEntity> All<TEntity, TOther, TOthers>(
            this Property<TEntity, TOthers> property,
            ODataFilter<TOther> filter
        )
            where TEntity : IEntity
            where TOthers : IEnumerable<TOther>
            where TOther : IEntity
        {
            var body = CheckLambdaBody(filter, nameof(filter));
            return Lambda(property, "all", body);
        }

        private static ODataFilter<TEntity> Lambda<TEntity>(IProperty<TEntity> property, string function, IODataLambdaBody body)
            where TEntity : IEntity
        {
            var expression = new ODataLambdaExpression<TEntity>(property, function, body);
            return new ODataFilter<TEntity>(expression);
        }

        private static IODataLambdaBody CheckLambdaBody<TOther>(ODataFilter<TOther> filter, string paramName) where TOther : IEntity
        {
            if (filter.Expression is not IODataLambdaBody body)
            {
                throw new ArgumentException(
                    $"The filter must be a valid lambda body expression, was: {filter.Expression} ({filter.Expression.GetType()})",
                    paramName
                );
            }

            return body;
        }
    }
}