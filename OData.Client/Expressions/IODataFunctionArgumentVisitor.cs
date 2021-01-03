namespace OData.Client.Expressions
{
    public interface IODataFunctionArgumentVisitor
    {
        void Visit(ODataConstantExpression expression);
    }
}