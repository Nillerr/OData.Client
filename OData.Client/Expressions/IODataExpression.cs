namespace OData.Client.Expressions
{
    public interface IODataExpression<TEntity>
        where TEntity : IEntity
    {
        void Visit(IODataExpressionVisitor<TEntity> visitor);
    }
}