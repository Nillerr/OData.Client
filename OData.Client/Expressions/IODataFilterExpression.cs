namespace OData.Client.Expressions
{
    /// <summary>
    /// An expression that is valid as the root of a filter.
    /// </summary>
    public interface IODataFilterExpression
    {
        /// <summary>
        /// Visits the expression, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataFilterExpressionVisitor visitor);
    }
}