namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents an expression valid as the operand of a <see cref="ODataLogicalExpression{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IODataLogicalOperand<TEntity> : IODataExpression<TEntity> where TEntity : IEntity
    {
        void Visit(IODataLogicalOperandVisitor<TEntity> visitor);
    }
}