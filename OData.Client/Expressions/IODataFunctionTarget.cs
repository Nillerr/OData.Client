namespace OData.Client.Expressions
{
    public interface IODataFunctionTarget<TEntity> : IODataExpression<TEntity> where TEntity : IEntity
    {
        void Visit(IODataFunctionTargetVisitor<TEntity> visitor);
    }
}