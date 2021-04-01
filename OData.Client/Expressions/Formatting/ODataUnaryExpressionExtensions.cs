namespace OData.Client.Expressions.Formatting
{
    internal static class ODataUnaryExpressionExtensions
    {
        public static string ToFilterString(
            this ODataUnaryExpression expression,
            string propertyPrefix,
            IExpressionFormatter expressionFormatter
        )
        {
            var operandVisitor = new FilterExpressionToStringVisitor(propertyPrefix, expressionFormatter);
            expression.Operand.Visit(operandVisitor);

            var operand = operandVisitor.ToString();
            var filterString = $"{expression.Operator} {operand}";
            return filterString;
        }
    }
}