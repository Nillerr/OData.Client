namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents a property
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataPropertyExpression : IODataBinaryLeftOperand,
        IODataBinaryRightOperand,
        IODataFunctionTarget 
    {
        public ODataPropertyExpression(IProperty property)
        {
            Property = property;
        }

        public IProperty Property { get; }

        public void Visit(IODataBinaryRightOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataBinaryLeftOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFunctionTargetVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Property)}: {Property}";
        }
    }
}