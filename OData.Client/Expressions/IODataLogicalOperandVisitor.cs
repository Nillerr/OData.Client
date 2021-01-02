namespace OData.Client.Expressions
{
    public interface IODataLogicalOperandVisitor<TEntity> where TEntity : IEntity
    {
        void Visit(ODataBinaryExpression<TEntity> expression);
        void Visit(ODataFunctionExpression<TEntity> expression);
        void Visit(ODataLambdaExpression<TEntity> expression);
        void Visit(ODataLogicalExpression<TEntity> expression);
        void Visit(ODataUnaryExpression<TEntity> expression);
    }
}