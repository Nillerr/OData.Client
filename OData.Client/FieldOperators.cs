using System;
using System.Collections.Generic;
using OData.Client.Expressions;
using OData.Client.Expressions.Functions;

namespace OData.Client
{
    public static class FieldOperators
    {
        public static Field<TEntity, TValue> Filter<TEntity, TOther, TValue>(
            this Field<TEntity, TOther?> field,
            Field<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return $"{field.Name}/{other.Name}";
        }

        public static ODataFilter<TEntity> EqualTo<TEntity, TValue>(this Field<TEntity, TValue> field, TValue value)
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression<TEntity>(field);
            var right = new ODataConstantExpression<TEntity>(value);
            var expression = new ODataBinaryExpression<TEntity>(left, "eq", right);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> NotEqualTo<TEntity, TValue>(this Field<TEntity, TValue> field, TValue value)
            where TEntity : IEntity
        {
            var left = new ODataPropertyExpression<TEntity>(field);
            var right = new ODataConstantExpression<TEntity>(value);
            var expression = new ODataBinaryExpression<TEntity>(left, "ne", right);
            return new ODataFilter<TEntity>(expression);
        }

        #nullable disable
        public static ODataFilter<TEntity> Contains<TEntity>(this Field<TEntity, string> field, string value)
            where TEntity : IEntity
        {
            return field.Function(ODataStringContainsFunction<TEntity>.Instance, value);
        }

        public static ODataFilter<TEntity> EndsWith<TEntity>(this Field<TEntity, string> field, string value)
            where TEntity : IEntity
        {
            return field.Function(ODataStringEndsWithFunction<TEntity>.Instance, value);
        }

        public static ODataFilter<TEntity> StartsWith<TEntity>(this Field<TEntity, string> field, string value)
            where TEntity : IEntity
        {
            return field.Function(ODataStringStartsWithFunction<TEntity>.Instance, value);
        }
        #nullable restore

        private static ODataFilter<TEntity> Function<TEntity, TValue>(
            this Field<TEntity, TValue> field,
            IODataFunction<TEntity> function,
            TValue value
        )
            where TEntity : IEntity
        {
            var target = new ODataPropertyExpression<TEntity>(field);
            var argument = new ODataConstantExpression<TEntity>(value);
            var expression = new ODataFunctionExpression<TEntity>(function, target, argument);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> Any<TEntity, TOther, TOthers>(
            this Field<TEntity, TOthers> field,
            ODataFilter<TOther> filter
        )
            where TEntity : IEntity
            where TOthers : IEnumerable<TOther>
            where TOther : IEntity
        {
            if (filter.Expression is not IODataLambdaBody body)
            {
                throw new ArgumentException($"The filter must be a valid lambda body expression, was: {filter.Expression} ({filter.Expression.GetType()})", nameof(filter));
            }
            
            var expression = new ODataLambdaExpression<TEntity>(field, "any", body);
            return new ODataFilter<TEntity>(expression);
        }
    }
}