namespace OData.Client.Expressions
{
    /// <summary>
    /// Used for `{left} and {right}` and `{left} or {right}`
    /// </summary>
    public class ODataLogicalExpression : IODataLogicalOperand, IODataFilterExpression, IODataLambdaBody
    {
        public ODataLogicalExpression(
            IODataLogicalOperand left,
            string @operator,
            IODataLogicalOperand right
        )
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public IODataLogicalOperand Left { get; }
        public string Operator { get; }
        public IODataLogicalOperand Right { get; }

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

        public override string ToString()
        {
            return $"{nameof(Left)}: {Left}, {nameof(Operator)}: {Operator}, {nameof(Right)}: {Right}";
        }
    }
}