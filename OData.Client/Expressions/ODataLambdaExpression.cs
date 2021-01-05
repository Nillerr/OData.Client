namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents a lambda expression, used for `{field}/{function}(o:{body})`
    /// </summary>
    public class ODataLambdaExpression : IODataFilterExpression, IODataLogicalOperand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataLambdaExpression"/> class.
        /// </summary>
        /// <param name="target">The target of the function.</param>
        /// <param name="function">The name of the function.</param>
        /// <param name="body">The body of the lambda expression.</param>
        public ODataLambdaExpression(IRefs target, string function, IODataLambdaBody body)
        {
            Function = function;
            Body = body;
            Target = target;
        }

        /// <summary>
        /// The target of the function.
        /// </summary>
        public IRefs Target;

        /// <summary>
        /// The name of the function.
        /// </summary>
        public string Function { get; }

        /// <summary>
        /// The body of the lambda expression.
        /// </summary>
        public IODataLambdaBody Body { get; }

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
            return $"{Target.Name}.{Function}(arg => {Body})";
        }
    }
}