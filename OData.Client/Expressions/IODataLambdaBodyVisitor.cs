namespace OData.Client.Expressions
{
    public interface IODataLambdaBodyVisitor
    {
        void Visit(ODataBinaryExpression expression);
        void Visit(ODataFunctionExpression expression);
        void Visit(ODataLogicalExpression expression);
        void Visit(ODataUnaryExpression expression);
    }
}