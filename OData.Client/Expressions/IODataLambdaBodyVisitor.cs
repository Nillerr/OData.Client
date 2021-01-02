namespace OData.Client.Expressions
{
    public interface IODataLambdaBodyVisitor
    {
        void Visit<TEntity>(ODataBinaryExpression<TEntity> expression) where TEntity : IEntity;
        void Visit<TEntity>(ODataFunctionExpression<TEntity> expression) where TEntity : IEntity;
        void Visit<TEntity>(ODataLogicalExpression<TEntity> expression) where TEntity : IEntity;
        void Visit<TEntity>(ODataUnaryExpression<TEntity> expression) where TEntity : IEntity;
    }
}