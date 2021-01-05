namespace OData.Client.Expressions.Formatting
{
    internal static class ODataUnaryExpressionExtensions
    {
        public static string ToFilterString(
            this ODataUnaryExpression expression,
            string propertyPrefix,
            IValueFormatter valueFormatter
        )
        {
            var operandVisitor = new FilterExpressionToStringVisitor(propertyPrefix, valueFormatter);
            expression.Operand.Visit(operandVisitor);

            var operand = operandVisitor.ToString();
            var filterString = $"{expression.Operator} {operand}";
            return filterString;
        }
    }
}