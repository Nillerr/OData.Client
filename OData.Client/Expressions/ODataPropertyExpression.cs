namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents a property
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataPropertyExpression<TEntity> : IODataExpression<TEntity>,
        IODataBinaryLeftOperand<TEntity>,
        IODataBinaryRightOperand<TEntity>,
        IODataFunctionTarget<TEntity> 
        where TEntity : IEntity
    {
        public ODataPropertyExpression(IField<TEntity> property)
        {
            Property = property;
        }

        public IField<TEntity> Property { get; }

        public void Visit(IODataExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataBinaryRightOperandVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataBinaryLeftOperandVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFunctionTargetVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Property)}: {Property}";
        }
    }
}