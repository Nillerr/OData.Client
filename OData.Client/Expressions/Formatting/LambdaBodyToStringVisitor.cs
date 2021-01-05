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
            PropertyPrefix = ParameterName + "/";
            
            _valueFormatter = valueFormatter;
        }

        public string ParameterName { get; }

        public string PropertyPrefix { get; }

        public void Visit(ODataBinaryExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _valueFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataFunctionExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _valueFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataLogicalExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _valueFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataUnaryExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _valueFormatter);
            _stringBuilder.Append(filterString);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}