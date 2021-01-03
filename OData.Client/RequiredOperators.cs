using System;
using System.Collections.Generic;
using OData.Client.Expressions;
using OData.Client.Expressions.Functions;

namespace OData.Client
{
    public static class RequiredOperators
    {
        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(
            this Required<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "eq", value);
        }

        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(
            this Required<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "ne", value);
        }

        public static ODataFilter<TEntity> GreaterThan<TEntity, TValue>(
            this Required<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "gt", value);
        }

        public static ODataFilter<TEntity> LessThan<TEntity, TValue>(
            this Required<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "lt", value);
        }

        public static ODataFilter<TEntity> GreaterThanOrEqualTo<TEntity, TValue>(
            this Required<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "ge", value);
        }

        public static ODataFilter<TEntity> LessThanOrEqualTo<TEntity, TValue>(
            this Required<TEntity, TValue> property,
            TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            return Binary(property, "le", value);
        }

        private static ODataFilter<TEntity> Binary<TEntity, TValue>(
            Required<TEntity, TValue> property,
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

#nullable disable
        public static ODataFilter<TEntity> Contains<TEntity>(this Required<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringContainsFunction.Instance, value);
        }

        public static ODataFilter<TEntity> EndsWith<TEntity>(this Required<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringEndsWithFunction.Instance, value);
        }

        public static ODataFilter<TEntity> StartsWith<TEntity>(this Required<TEntity, string> property, string value)
            where TEntity : IEntity
        {
            return property.Function(ODataStringStartsWithFunction.Instance, value);
        }
#nullable restore

        private static ODataFilter<TEntity> Function<TEntity, TValue>(
            this Required<TEntity, TValue> property,
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

        public static ODataFilter<TEntity> Any<TEntity, TOther, TOthers>(
            this Required<TEntity, TOthers> property,
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
            this Required<TEntity, TOthers> property,
            ODataFilter<TOther> filter
        )
            where TEntity : IEntity
            where TOthers : IEnumerable<TOther>
            where TOther : IEntity
        {
            var body = CheckLambdaBody(filter, nameof(filter));
            return Lambda(property, "all", body);
        }

        private static ODataFilter<TEntity> Lambda<TEntity>(
            IProperty<TEntity> property,
            string function,
            IODataLambdaBody body
        )
            where TEntity : IEntity
        {
            var expression = new ODataLambdaExpression(property, function, body);
            return new ODataFilter<TEntity>(expression);
        }

        private static IODataLambdaBody CheckLambdaBody<TOther>(ODataFilter<TOther> filter, string paramName)
            where TOther : IEntity
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