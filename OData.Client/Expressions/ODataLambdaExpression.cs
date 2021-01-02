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
        public ODataLambdaExpression(IField<TEntity> field, string function, IODataLambdaBody body)
        {
            Function = function;
            Body = body;
            Field = field;
        }

        public IField<TEntity> Field;

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
            return $"{nameof(Field)}: {Field}, {nameof(Function)}: {Function}, {nameof(Body)}: {Body}";
        }
    }
}