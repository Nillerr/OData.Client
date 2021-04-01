namespace OData.Client.Expressions.Formatting
{
    public class DefaultExpressionFormatter : IExpressionFormatter
    {
        private readonly IValueFormatter _valueFormatter;

        public DefaultExpressionFormatter(IValueFormatter valueFormatter)
        {
            _valueFormatter = valueFormatter;
        }

        public string ToString(ODataConstantExpression expression)
        {
            return _valueFormatter.Serialize(expression.Value);
        }

        public string ToString(ODataFunctionRequestArgument argument)
        {
            return _valueFormatter.Serialize(argument.Value);
        }
    }
}