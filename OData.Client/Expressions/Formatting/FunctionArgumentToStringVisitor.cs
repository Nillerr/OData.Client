using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class FunctionArgumentToStringVisitor : IODataFunctionArgumentVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IExpressionFormatter _expressionFormatter;

        public FunctionArgumentToStringVisitor(IExpressionFormatter expressionFormatter, string propertyPrefix)
        {
            _expressionFormatter = expressionFormatter;
            PropertyPrefix = propertyPrefix;
        }

        private string PropertyPrefix { get; }

        public void Visit(ODataConstantExpression expression)
        {
            var stringValue = _expressionFormatter.ToString(expression);
            _stringBuilder.Append(stringValue);
        }

        public void Visit(ODataPropertyExpression expression)
        {
            _stringBuilder.Append(PropertyPrefix + expression.Property.Name);
        }

        public void Clear()
        {
            _stringBuilder.Clear();
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}