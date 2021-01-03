namespace OData.Client.Expressions
{
    /// <summary>
    /// An expression that is valid as the operand of a <see cref="ODataLogicalExpression"/>.
    /// </summary>
    public interface IODataLogicalOperand
    {
        /// <summary>
        /// Visits the expression, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataLogicalOperandVisitor visitor);
    }
}