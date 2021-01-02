namespace OData.Client.Expressions
{
    public interface IODataBinaryLeftOperand<TEntity> : IODataExpression<TEntity> where TEntity : IEntity
    {
        void Visit(IODataBinaryLeftOperandVisitor<TEntity> visitor);
    }
}