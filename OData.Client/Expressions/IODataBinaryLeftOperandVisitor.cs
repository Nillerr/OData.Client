namespace OData.Client.Expressions
{
    public interface IODataBinaryLeftOperandVisitor
    {
        void Visit(ODataPropertyExpression expression);
    }
}