namespace OData.Client.Expressions
{
    public interface IODataFilterExpression<TEntity> : IODataExpression<TEntity> where TEntity : IEntity
    {
        void Visit(IODataFilterExpressionVisitor<TEntity> visitor);
    }
}