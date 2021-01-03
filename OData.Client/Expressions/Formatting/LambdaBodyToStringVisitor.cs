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

        public void Visit(ODataBinaryExpression expression)
        {
            var leftVisitor = new BinaryLeftOperandToStringVisitor($"{ParameterName}:");
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new BinaryRightOperandToStringVisitor($"{ParameterName}:", _valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            _stringBuilder.Append($"({left} {expression.Operator} {right})");
        }

        public void Visit(ODataFunctionExpression expression)
        {
            var targetVisitor = new FunctionTargetToStringVisitor(ParameterName + "/");
            expression.Target.Visit(targetVisitor);
            
            var argumentVisitor = new FunctionArgumentToStringVisitor(_valueFormatter);
            expression.Argument.Visit(argumentVisitor);

            var target = targetVisitor.ToString();
            var function = expression.Function.Name;
            var argument = argumentVisitor.ToString();

            _stringBuilder.Append($"{function}({target},{argument})");
        }

        public void Visit(ODataLogicalExpression expression)
        {
            var leftVisitor = new LogicalOperandToStringVisitor(ParameterName + "/", _valueFormatter);
            expression.Left.Visit(leftVisitor);
            
            var rightVisitor = new LogicalOperandToStringVisitor(ParameterName + "/", _valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            _stringBuilder.Append($"({left} {expression.Operator} {right})");
        }

        public void Visit(ODataUnaryExpression expression)
        {
            var operandVisitor = new FilterExpressionToStringVisitor(ParameterName + "/", _valueFormatter);
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