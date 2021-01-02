namespace OData.Client.Expressions
{
    public interface IODataBinaryLeftOperandVisitor<TEntity> where TEntity : IEntity
    {
        void Visit(ODataPropertyExpression<TEntity> expression);
    }
}