namespace OData.Client.Expressions
{
    public interface IODataBinaryRightOperand<TEntity> : IODataExpression<TEntity> where TEntity : IEntity
    {
        void Visit(IODataBinaryRightOperandVisitor<TEntity> visitor);
    }
}