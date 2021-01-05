using System.Linq;

namespace OData.Client.Expressions
{
    /// <summary>
    /// A function call expression invoked on a <see cref="Target"/> with a single <see cref="Argument"/>, such as
    /// <c>endswith(emailaddress1,'@contorso.com')</c>.
    /// </summary>
    /// <example>
    /// <code>
    /// public static IODataFilter&lt;TEntity&gt; EndsWith&lt;TEntity, string&gt;(
    ///     this IProperty&lt;TEntity, string&gt; property, string value)
    /// {
    ///     var expression = new ODataFunctionExpression&lt;TEntity&gt;("endswith", property.Name, value);
    ///     return new ODataFilter&lt;TEntity&gt;(expression);
    /// }
    /// </code>
    /// </example>
    public class ODataFunctionExpression : IODataLogicalOperand, IODataFilterExpression, IODataLambdaBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataFunctionExpression"/> class.
        /// </summary>
        /// <param name="function">The function to apply.</param>
        /// <param name="arguments">The arguments of the function invocation.</param>
        public ODataFunctionExpression(
            IODataFunction function,
            params IODataFunctionArgument[] arguments
        )
        {
            Function = function;
            Arguments = arguments;
        }

        /// <summary>
        /// The name of the function.
        /// </summary>
        public IODataFunction Function { get; }
        
        /// <summary>
        /// The arguments of the function invocation.
        /// </summary>
        public IODataFunctionArgument[] Arguments { get; }

        public void Visit(IODataLambdaBodyVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLogicalOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Function)}: {Function}, {nameof(Arguments)}: {string.Join(",", Arguments.Select(argument => argument.ToString()))}";
        }
    }
}