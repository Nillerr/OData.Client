using OData.Client.Expressions.Formatting;

namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents a lambda expression, used for `{field}/{function}(o:{body})`
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataLambdaExpression<TEntity> : IODataExpression<TEntity>,
        IODataFilterExpression<TEntity>,
        IODataLogicalOperand<TEntity>
        where TEntity : IEntity
    {
        public ODataLambdaExpression(IProperty<TEntity> property, string function, IODataLambdaBody body)
        {
            Function = function;
            Body = body;
            Property = property;
        }

        public IProperty<TEntity> Property;

        public string Function { get; }

        public IODataLambdaBody Body { get; }

        public void Visit(IODataExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLogicalOperandVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Property)}: {Property}, {nameof(Function)}: {Function}, {nameof(Body)}: {Body}";
        }
    }
}