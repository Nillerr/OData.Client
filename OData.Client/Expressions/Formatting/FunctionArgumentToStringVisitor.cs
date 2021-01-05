using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class FunctionArgumentToStringVisitor : IODataFunctionArgumentVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;

        public FunctionArgumentToStringVisitor(IValueFormatter valueFormatter, string propertyPrefix)
        {
            _valueFormatter = valueFormatter;
            PropertyPrefix = propertyPrefix;
        }

        private string PropertyPrefix { get; }

        public void Visit(ODataConstantExpression expression)
        {
            var stringValue = _valueFormatter.ToString(expression);
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