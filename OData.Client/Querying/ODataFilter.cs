using System;
using OData.Client.Expressions;

namespace OData.Client
{
    public readonly struct ODataFilter<TEntity> : IEquatable<ODataFilter<TEntity>>
        where TEntity : IEntity
    {
        public static ODataFilter<TEntity> Empty => default;
        
        public IODataFilterExpression? Expression { get; }

        public ODataFilter(IODataFilterExpression expression) => Expression = expression;

        public bool Equals(ODataFilter<TEntity> other) => Equals(Expression, other.Expression);
        public override bool Equals(object? obj) => obj is ODataFilter<TEntity> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Expression);

        public override string? ToString() => Expression?.ToString();
        
        public static bool operator ==(ODataFilter<TEntity> left, ODataFilter<TEntity> right) => left.Equals(right);
        public static bool operator !=(ODataFilter<TEntity> left, ODataFilter<TEntity> right) => !left.Equals(right);
        
        public static ODataFilter<TEntity> operator &(ODataFilter<TEntity> left, ODataFilter<TEntity> right)
        {
            if (left.Expression == null) return right;
            if (right.Expression == null) return left;

            var leftOperand = CheckOperand(left.Expression, nameof(left));
            var rightOperand = CheckOperand(right.Expression, nameof(right));

            var expression = new ODataLogicalExpression(leftOperand, "and", rightOperand);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> operator |(ODataFilter<TEntity> left, ODataFilter<TEntity> right)
        {
            if (left.Expression == null) return right;
            if (right.Expression == null) return left;

            var leftOperand = CheckOperand(left.Expression, nameof(left));
            var rightOperand = CheckOperand(right.Expression, nameof(right));

            var expression = new ODataLogicalExpression(leftOperand, "or", rightOperand);
            return new ODataFilter<TEntity>(expression);
        }

        public static ODataFilter<TEntity> operator !(ODataFilter<TEntity> filter)
        {
            if (filter.Expression == null) return filter;

            var expression = new ODataUnaryExpression("not", filter.Expression);
            return new ODataFilter<TEntity>(expression);
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