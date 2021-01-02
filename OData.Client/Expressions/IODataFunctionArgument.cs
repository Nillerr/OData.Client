namespace OData.Client.Expressions
{
    public interface IODataFunctionArgument<TEntity> : IODataExpression<TEntity> where TEntity : IEntity
    {
        void Visit(IODataFunctionArgumentVisitor<TEntity> visitor);
    }
}