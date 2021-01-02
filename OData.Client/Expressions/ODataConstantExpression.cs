namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents a constant value
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataConstantExpression<TEntity> : IODataExpression<TEntity>,
        IODataBinaryRightOperand<TEntity>,
        IODataFunctionArgument<TEntity>
        where TEntity : IEntity
    {
        public ODataConstantExpression(object? value)
        {
            Value = value;
        }

        public object? Value { get; }

        public void Visit(IODataExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataBinaryRightOperandVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFunctionArgumentVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }
}