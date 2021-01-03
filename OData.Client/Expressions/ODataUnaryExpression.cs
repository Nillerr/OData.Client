namespace OData.Client.Expressions
{
    /// <summary>
    /// Used for the `not` operator.
    /// </summary>
    public class ODataUnaryExpression : IODataFilterExpression, IODataLogicalOperand, IODataLambdaBody
    {
        public ODataUnaryExpression(string @operator, IODataFilterExpression operand)
        {
            Operator = @operator;
            Operand = operand;
        }

        public string Operator { get; }
        public IODataFilterExpression Operand { get; }

        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLogicalOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Operator)}: {Operator}, {nameof(Operand)}: {Operand}";
        }
    }
}