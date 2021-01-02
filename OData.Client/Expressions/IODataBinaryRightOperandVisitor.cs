namespace OData.Client.Expressions
{
    public interface IODataBinaryRightOperandVisitor<TEntity> where TEntity : IEntity
    {
        void Visit(ODataConstantExpression<TEntity> expression);
        void Visit(ODataPropertyExpression<TEntity> expression);
    }
}