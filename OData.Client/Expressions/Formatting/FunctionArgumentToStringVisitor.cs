using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class FunctionArgumentToStringVisitor : IODataFunctionArgumentVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;

        public FunctionArgumentToStringVisitor(IValueFormatter valueFormatter)
        {
            _valueFormatter = valueFormatter;
        }

        public void Visit(ODataConstantExpression expression)
        {
            var stringValue = _valueFormatter.ToString(expression);
            _stringBuilder.Append(stringValue);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}