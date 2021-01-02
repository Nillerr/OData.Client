namespace OData.Client.Expressions
{
    public interface IODataExpressionVisitor<TEntity>
        where TEntity : IEntity
    {
        void Visit(ODataBinaryExpression<TEntity> expression);
        void Visit(ODataConstantExpression<TEntity> expression);
        void Visit(ODataLogicalExpression<TEntity> expression);
        void Visit(ODataPropertyExpression<TEntity> expression);
        void Visit(ODataFunctionExpression<TEntity> expression);
        void Visit(ODataUnaryExpression<TEntity> expression);
        void Visit(ODataLambdaExpression<TEntity> expression);
    }
}