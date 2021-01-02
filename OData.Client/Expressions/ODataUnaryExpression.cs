namespace OData.Client.Expressions
{
    /// <summary>
    /// Used for the `not` operator.
    /// </summary>
    public class ODataUnaryExpression<TEntity> : IODataExpression<TEntity>,
        IODataFilterExpression<TEntity>,
        IODataLogicalOperand<TEntity>,
        IODataLambdaBody
        where TEntity : IEntity
    {
        public ODataUnaryExpression(string @operator, IODataFilterExpression<TEntity> operand)
        {
            Operator = @operator;
            Operand = operand;
        }

        public string Operator { get; }
        public IODataFilterExpression<TEntity> Operand { get; }

        public void Visit(IODataExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLogicalOperandVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Operator)}: {Operator}, {nameof(Operand)}: {Operand}";
        }
    }
}