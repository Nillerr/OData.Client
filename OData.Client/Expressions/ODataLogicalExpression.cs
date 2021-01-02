namespace OData.Client.Expressions
{
    /// <summary>
    /// Used for `{left} and {right}` and `{left} or {right}`
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataLogicalExpression<TEntity> : IODataExpression<TEntity>,
        IODataLogicalOperand<TEntity>,
        IODataFilterExpression<TEntity>,
        IODataLambdaBody
        where TEntity : IEntity
    {
        public ODataLogicalExpression(
            IODataLogicalOperand<TEntity> left,
            string @operator,
            IODataLogicalOperand<TEntity> right
        )
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public IODataLogicalOperand<TEntity> Left { get; }
        public string Operator { get; }
        public IODataLogicalOperand<TEntity> Right { get; }

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