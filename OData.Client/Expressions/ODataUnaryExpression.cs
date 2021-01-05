namespace OData.Client.Expressions
{
    /// <summary>
    /// <para>
    ///     An expression accepting an operator and a single operand to apply that operator to.
    /// </para>
    /// <para>
    ///     Used for <c>not {operand}</c>.
    /// </para> 
    /// </summary>
    public class ODataUnaryExpression : IODataFilterExpression, IODataLogicalOperand, IODataLambdaBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataUnaryExpression"/> class.
        /// </summary>
        /// <param name="operator">The operator.</param>
        /// <param name="operand">The operand.</param>
        public ODataUnaryExpression(string @operator, IODataFilterExpression operand)
        {
            Operator = @operator;
            Operand = operand;
        }

        /// <summary>
        /// The operator.
        /// </summary>
        public string Operator { get; }
        
        /// <summary>
        /// The operand.
        /// </summary>
        public IODataFilterExpression Operand { get; }

        /// <inheritdoc />
        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public void Visit(IODataFilterExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public void Visit(IODataLogicalOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Operator)}: {Operator}, {nameof(Operand)}: {Operand}";
        }
    }
}