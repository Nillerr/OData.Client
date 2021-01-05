using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class BinaryRightOperandToStringVisitor : IODataBinaryRightOperandVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;
        
        public BinaryRightOperandToStringVisitor(string propertyPrefix, IValueFormatter valueFormatter)
        {
            PropertyPrefix = propertyPrefix;
            _valueFormatter = valueFormatter;
        }

        public string PropertyPrefix { get; }

        public void Visit(ODataConstantExpression expression)
        {
            var stringValue = _valueFormatter.ToString(expression);
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