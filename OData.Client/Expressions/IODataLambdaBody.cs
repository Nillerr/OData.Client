namespace OData.Client.Expressions
{
    public interface IODataLambdaBody
    {
        void Visit(IODataLambdaBodyVisitor visitor);
    }
}