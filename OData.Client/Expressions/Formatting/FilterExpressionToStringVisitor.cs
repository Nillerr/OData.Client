using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal class FilterExpressionToStringVisitor : IODataFilterExpressionVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        
        private readonly IExpressionFormatter _expressionFormatter;

        public FilterExpressionToStringVisitor(string propertyPrefix, IExpressionFormatter expressionFormatter)
        {
            PropertyPrefix = propertyPrefix;
            _expressionFormatter = expressionFormatter;
        }

        private string PropertyPrefix { get; }
        
        public void Visit(ODataBinaryExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _expressionFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataFunctionExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _expressionFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataLambdaExpression expression)
        {
            var filterString = expression.ToFilterString(_expressionFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataLogicalExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _expressionFormatter);
            _stringBuilder.Append(filterString);
        }

        public void Visit(ODataUnaryExpression expression)
        {
            var filterString = expression.ToFilterString(PropertyPrefix, _expressionFormatter);
            _stringBuilder.Append(filterString);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}