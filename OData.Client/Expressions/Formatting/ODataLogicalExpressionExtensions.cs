namespace OData.Client.Expressions.Formatting
{
    internal static class ODataLogicalExpressionExtensions
    {
        public static string ToFilterString(
            this ODataLogicalExpression expression,
            string propertyPrefix,
            IExpressionFormatter expressionFormatter
        )
        {
            var leftVisitor = new LogicalOperandToStringVisitor(propertyPrefix, expressionFormatter);
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new LogicalOperandToStringVisitor(propertyPrefix, expressionFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            var filterString = $"({left} {expression.Operator} {right})";
            return filterString;
        }
    }
}