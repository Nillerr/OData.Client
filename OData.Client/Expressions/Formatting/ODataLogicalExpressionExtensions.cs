namespace OData.Client.Expressions.Formatting
{
    internal static class ODataLogicalExpressionExtensions
    {
        public static string ToFilterString(
            this ODataLogicalExpression expression,
            string propertyPrefix,
            IValueFormatter valueFormatter
        )
        {
            var leftVisitor = new LogicalOperandToStringVisitor(propertyPrefix, valueFormatter);
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new LogicalOperandToStringVisitor(propertyPrefix, valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            var filterString = $"({left} {expression.Operator} {right})";
            return filterString;
        }
    }
}