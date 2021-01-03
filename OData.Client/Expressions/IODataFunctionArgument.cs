namespace OData.Client.Expressions
{
    /// <summary>
    /// A function argument that is valid as the argument of a <see cref="ODataFunctionExpression"/>.
    /// </summary>
    public interface IODataFunctionArgument
    {
        /// <summary>
        /// Visits the function argument, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataFunctionArgumentVisitor visitor);
    }
}