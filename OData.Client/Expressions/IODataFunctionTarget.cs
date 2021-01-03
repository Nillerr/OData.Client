namespace OData.Client.Expressions
{
    /// <summary>
    /// A function target (the first argument) that is valid as the target of a <see cref="ODataFunctionExpression"/>.
    /// </summary>
    public interface IODataFunctionTarget
    {
        /// <summary>
        /// Visits the function target, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataFunctionTargetVisitor visitor);
    }
}