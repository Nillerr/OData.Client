namespace OData.Client.Expressions
{
    /// <summary>
    /// An expression that is valid as the body of a <see cref="ODataLambdaExpression"/>.
    /// </summary>
    public interface IODataLambdaBody
    {
        /// <summary>
        /// Visits the lambda body, invoking the corresponding method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Visit(IODataLambdaBodyVisitor visitor);
    }
}