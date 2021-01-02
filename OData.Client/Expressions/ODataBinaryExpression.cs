namespace OData.Client.Expressions
{
    public class ODataBinaryExpression<TEntity> : IODataLogicalOperand<TEntity>,
        IODataFilterExpression<TEntity>,
        IODataLambdaBody
        where TEntity : IEntity
    {
        public ODataBinaryExpression(
            IODataBinaryLeftOperand<TEntity> left,
            string @operator,
            IODataBinaryRightOperand<TEntity> right
        )
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public IODataBinaryLeftOperand<TEntity> Left { get; }
        public string Operator { get; }
        public IODataBinaryRightOperand<TEntity> Right { get; }

        public void Visit(IODataExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLogicalOperandVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Left)}: {Left}, {nameof(Operator)}: {Operator}, {nameof(Right)}: {Right}";
        }
    }
}