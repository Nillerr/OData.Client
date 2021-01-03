namespace OData.Client.Expressions
{
    /// <summary>
    /// A visitor for implementors of <see cref="IODataFilterExpression"/>.
    /// </summary>
    public interface IODataFilterExpressionVisitor
    {
        void Visit(ODataBinaryExpression expression);
        void Visit(ODataFunctionExpression expression);
        void Visit(ODataLambdaExpression expression);
        void Visit(ODataLogicalExpression expression);
        void Visit(ODataUnaryExpression expression);
    }
}