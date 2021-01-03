namespace OData.Client.Expressions
{
    public interface IODataLogicalOperandVisitor
    {
        void Visit(ODataBinaryExpression expression);
        void Visit(ODataFunctionExpression expression);
        void Visit(ODataLambdaExpression expression);
        void Visit(ODataLogicalExpression expression);
        void Visit(ODataUnaryExpression expression);
    }
}