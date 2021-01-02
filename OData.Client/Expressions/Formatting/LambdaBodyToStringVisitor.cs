using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class LambdaBodyToStringVisitor : IODataLambdaBodyVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;

        public LambdaBodyToStringVisitor(string parameterName, IValueFormatter valueFormatter)
        {
            ParameterName = parameterName;
            _valueFormatter = valueFormatter;
        }

        public string ParameterName { get; }

        public void Visit<TEntity>(ODataBinaryExpression<TEntity> expression) where TEntity : IEntity
        {
            var leftVisitor = new BinaryLeftOperandToStringVisitor<TEntity>($"{ParameterName}:");
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new BinaryRightOperandToStringVisitor<TEntity>($"{ParameterName}:", _valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            _stringBuilder.Append($"({left} {expression.Operator} {right})");
        }

        public void Visit<TEntity>(ODataFunctionExpression<TEntity> expression) where TEntity : IEntity
        {
            var targetVisitor = new FunctionTargetToStringVisitor<TEntity>(ParameterName + "/");
            expression.Target.Visit(targetVisitor);
            
            var argumentVisitor = new FunctionArgumentToStringVisitor<TEntity>(_valueFormatter);
            expression.Argument.Visit(argumentVisitor);

            var target = targetVisitor.ToString();
            var function = expression.Function.Name;
            var argument = argumentVisitor.ToString();

            _stringBuilder.Append($"{function}({target},{argument})");
        }

        public void Visit<TEntity>(ODataLogicalExpression<TEntity> expression) where TEntity : IEntity
        {
            var leftVisitor = new LogicalOperandToStringVisitor<TEntity>(ParameterName + "/", _valueFormatter);
            expression.Left.Visit(leftVisitor);
            
            var rightVisitor = new LogicalOperandToStringVisitor<TEntity>(ParameterName + "/", _valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            _stringBuilder.Append($"({left} {expression.Operator} {right})");
        }

        public void Visit<TEntity>(ODataUnaryExpression<TEntity> expression) where TEntity : IEntity
        {
            var operandVisitor = new FilterExpressionToStringVisitor<TEntity>(ParameterName + "/", _valueFormatter);
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