namespace OData.Client.Expressions
{
    /// <summary>
    /// A property expression.
    /// </summary>
    public class ODataPropertyExpression :
        IODataBinaryLeftOperand,
        IODataBinaryRightOperand,
        IODataFunctionArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataPropertyExpression"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public ODataPropertyExpression(IProperty property)
        {
            Property = property;
        }

        /// <summary>
        /// The property.
        /// </summary>
        public IProperty Property { get; }

        /// <inheritdoc />
        public void Visit(IODataBinaryRightOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public void Visit(IODataBinaryLeftOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public void Visit(IODataFunctionArgumentVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Property)}: {Property}";
        }
    }
}