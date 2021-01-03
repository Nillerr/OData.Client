using System;
using OData.Client.Expressions;

namespace OData.Client
{
    public readonly struct ODataFilter<TEntity> : IEquatable<ODataFilter<TEntity>>
        where TEntity : IEntity
    {
        public IODataFilterExpression Expression { get; }

        public ODataFilter(IODataFilterExpression expression)
        {
            Expression = expression;
        }

        public bool Equals(ODataFilter<TEntity> other)
        {
            return Expression == other.Expression;
        }

        public override bool Equals(object? obj)
        {
            return obj is ODataFilter<TEntity> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Expression.GetHashCode();
        }

        public static bool operator ==(ODataFilter<TEntity> left, ODataFilter<TEntity> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ODataFilter<TEntity> left, ODataFilter<TEntity> right)
        {
            return !left.Equals(right);
        }

        public static ODataFilter<TEntity> operator &(ODataFilter<TEntity> left, ODataFilter<TEntity> right)
        {
            var leftOperand = CheckOperand(left, nameof(left));
            var rightOperand = CheckOperand(right, nameof(right));

            var expression = new ODataLogicalExpression(leftOperand, "and", rightOperand);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> operator |(ODataFilter<TEntity> left, ODataFilter<TEntity> right)
        {
            var leftOperand = CheckOperand(left, nameof(left));
            var rightOperand = CheckOperand(right, nameof(right));

            var expression = new ODataLogicalExpression(leftOperand, "or", rightOperand);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> operator !(ODataFilter<TEntity> filter)
        {
            var expression = new ODataUnaryExpression("not", filter.Expression);
            return new ODataFilter<TEntity>(expression);
        }

        public override string? ToString()
        {
            return Expression.ToString();
        }

        private static IODataLogicalOperand CheckOperand(ODataFilter<TEntity> filter, string paramName)
        {
            return CheckOperand(filter.Expression, paramName);
        }

        private static IODataLogicalOperand CheckOperand(IODataFilterExpression expression, string paramName)
        {
            if (expression is not IODataLogicalOperand operand)
            {
                throw new ArgumentException($"The expression must be a valid logical operand, was: {expression} ({expression.GetType()})", paramName);
            }

            return operand;
        }
    }
}