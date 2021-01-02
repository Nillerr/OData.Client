namespace OData.Client.Expressions
{
    public interface IODataFunctionTargetVisitor<TEntity> where TEntity : IEntity
    {
        void Visit(ODataPropertyExpression<TEntity> expression);
    }
}