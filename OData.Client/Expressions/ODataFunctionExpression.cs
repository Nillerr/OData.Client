namespace OData.Client.Expressions
{
    /// <summary>
    /// Function call expression, used for `contains({field},'{value}')`, `startswith({field},'{value}')` and `endswith({field},'{value}')`
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ODataFunctionExpression<TEntity> : IODataLogicalOperand<TEntity>,
        IODataFilterExpression<TEntity>,
        IODataLambdaBody
        where TEntity : IEntity
    {
        public ODataFunctionExpression(
            IODataFunction<TEntity> function,
            IODataFunctionTarget<TEntity> target,
            IODataFunctionArgument<TEntity> argument
        )
        {
            Function = function;
            Target = target;
            Argument = argument;
        }

        public IODataFunction<TEntity> Function { get; }
        public IODataFunctionTarget<TEntity> Target { get; }
        public IODataFunctionArgument<TEntity> Argument { get; }

        public void Visit(IODataExpressionVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataLambdaBodyVisitor visitor)
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
            return $"{nameof(Function)}: {Function}, {nameof(Target)}: {Target}, {nameof(Argument)}: {Argument}";
        }
    }
}