namespace OData.Client.Expressions
{
    public interface IODataFunctionArgumentVisitor<TEntity> where TEntity : IEntity
    {
        void Visit(ODataConstantExpression<TEntity> expression);
    }
}