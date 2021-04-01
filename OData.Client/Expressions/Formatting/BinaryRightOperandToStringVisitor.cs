using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class BinaryRightOperandToStringVisitor : IODataBinaryRightOperandVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IExpressionFormatter _expressionFormatter;
        
        public BinaryRightOperandToStringVisitor(string propertyPrefix, IExpressionFormatter expressionFormatter)
        {
            PropertyPrefix = propertyPrefix;
            _expressionFormatter = expressionFormatter;
        }

        private string PropertyPrefix { get; }

        public void Visit(ODataConstantExpression expression)
        {
            var stringValue = _expressionFormatter.ToString(expression);
            _stringBuilder.Append(stringValue);
        }

        public void Visit(ODataPropertyExpression expression)
        {
            _stringBuilder.Append(PropertyPrefix + expression.Property.SelectableName);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}