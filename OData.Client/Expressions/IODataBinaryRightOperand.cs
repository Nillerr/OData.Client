namespace OData.Client.Expressions
{
    /// <summary>
    /// An expression that is valid as the right operand of <see cref="ODataBinaryExpression"/>.
    /// </summary>
    public interface IODataBinaryRightOperand
    {
        /// <summary>
        /// Visits the expression, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataBinaryRightOperandVisitor visitor);
    }
}