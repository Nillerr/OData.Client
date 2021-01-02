using System.Text;

namespace OData.Client.Expressions.Formatting
{
    public class FilterExpressionToStringVisitor<TEntity> : IODataFilterExpressionVisitor<TEntity>
        where TEntity : IEntity
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;

        public FilterExpressionToStringVisitor(string propertyPrefix, IValueFormatter valueFormatter)
        {
            PropertyPrefix = propertyPrefix;
            _valueFormatter = valueFormatter;
        }

        public string PropertyPrefix { get; }
        
        public void Visit(ODataBinaryExpression<TEntity> expression)
        {
            var leftVisitor = new BinaryLeftOperandToStringVisitor<TEntity>(PropertyPrefix);
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new BinaryRightOperandToStringVisitor<TEntity>(PropertyPrefix, _valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            _stringBuilder.Append($"({left} {expression.Operator} {right})");
        }

        public void Visit(ODataFunctionExpression<TEntity> expression)
        {
            var targetVisitor = new FunctionTargetToStringVisitor<TEntity>(PropertyPrefix);
            expression.Target.Visit(targetVisitor);

            var argumentVisitor = new FunctionArgumentToStringVisitor<TEntity>(_valueFormatter);
            expression.Argument.Visit(argumentVisitor);

            var target = targetVisitor.ToString();
            var function = expression.Function.Name;
            var argument = argumentVisitor.ToString();

            _stringBuilder.Append($"{function}({target},{argument})");
        }

        public void Visit(ODataLambdaExpression<TEntity> expression)
        {
            var visitor = new LambdaBodyToStringVisitor("o", _valueFormatter);
            expression.Body.Visit(visitor);
            
            _stringBuilder.Append($"{expression.Property.Name}/{expression.Function}(o:{visitor})");
        }

        public void Visit(ODataLogicalExpression<TEntity> expression)
        {
            var leftVisitor = new LogicalOperandToStringVisitor<TEntity>(PropertyPrefix, _valueFormatter);
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new LogicalOperandToStringVisitor<TEntity>(PropertyPrefix, _valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            _stringBuilder.Append($"({left} {expression.Operator} {right})");
        }

        public void Visit(ODataUnaryExpression<TEntity> expression)
        {
            var operandVisitor = new FilterExpressionToStringVisitor<TEntity>(PropertyPrefix, _valueFormatter);
            expression.Operand.Visit(operandVisitor);

            var operand = operandVisitor.ToString();
            _stringBuilder.Append($"{expression.Operator} {operand}");
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}