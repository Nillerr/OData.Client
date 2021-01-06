namespace OData.Client.Expressions
{
    /// <summary>
    /// A binary comparison expression, e.g. <c>{left} eq {right}</c> or <c>{left} gt {right}</c>.
    /// </summary>
    /// <remarks>
    /// Logical expression (<c>not</c>, <c>and</c>, <c>or</c>) are represented by <see cref="ODataLogicalExpression"/>.
    /// </remarks>
    public class ODataBinaryExpression : IODataLogicalOperand, IODataFilterExpression, IODataLambdaBody
    {
        public ODataBinaryExpression(
            IODataBinaryLeftOperand left,
            string @operator,
            IODataBinaryRightOperand right
        )
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public IODataBinaryLeftOperand Left { get; }
        public string Operator { get; }
        public IODataBinaryRightOperand Right { get; }

        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLogicalOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({Left} {Operator} {Right})";
        }
    }
}