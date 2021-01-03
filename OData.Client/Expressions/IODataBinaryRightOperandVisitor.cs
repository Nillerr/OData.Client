namespace OData.Client.Expressions
{
    public interface IODataBinaryRightOperandVisitor
    {
        void Visit(ODataConstantExpression expression);
        void Visit(ODataPropertyExpression expression);
    }
}