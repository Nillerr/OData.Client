namespace OData.Client.Expressions
{
    /// <summary>
    /// <para>
    ///     A logical expression combines a left and right operand each evaluating to a boolean value, as well as an
    ///     operator to apply to the resulting boolean value of the left and right operand, resulting in a boolean
    ///     value itself.
    /// </para>
    /// <para>
    ///     Used for <c>{left} and {right}</c> and <c>{left} or {right}</c>.
    /// </para>
    /// </summary>
    public class ODataLogicalExpression : IODataLogicalOperand, IODataFilterExpression, IODataLambdaBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataLogicalExpression"/> class.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="right">The right operand.</param>
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

        /// <summary>
        /// The left operand.
        /// </summary>
        public IODataLogicalOperand Left { get; }
        
        /// <summary>
        /// The operator.
        /// </summary>
        public string Operator { get; }
        
        /// <summary>
        /// The right operand.
        /// </summary>
        public IODataLogicalOperand Right { get; }

        /// <inheritdoc />
        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public void Visit(IODataLogicalOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public void Visit(IODataFilterExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({Left} {Operator} {Right})";
        }
    }
}