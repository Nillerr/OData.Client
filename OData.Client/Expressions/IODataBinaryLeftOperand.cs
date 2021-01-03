namespace OData.Client.Expressions
{
    /// <summary>
    /// An expression that is valid as the left operand of <see cref="ODataBinaryExpression"/>.
    /// </summary>
    public interface IODataBinaryLeftOperand
    {
        /// <summary>
        /// Visits the expression, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataBinaryLeftOperandVisitor visitor);
    }
}