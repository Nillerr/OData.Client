namespace OData.Client.Expressions
{
    public interface IODataFunctionTargetVisitor
    {
        void Visit(ODataPropertyExpression expression);
    }
}