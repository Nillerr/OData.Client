namespace OData.Client.Expressions.Formatting
{
    internal static class ODataBinaryExpressionExtensions
    {
        public static string ToFilterString(
            this ODataBinaryExpression expression,
            string propertyPrefix,
            IValueFormatter valueFormatter
        )
        {
            var leftVisitor = new BinaryLeftOperandToStringVisitor(propertyPrefix);
            expression.Left.Visit(leftVisitor);

            var rightVisitor = new BinaryRightOperandToStringVisitor(propertyPrefix, valueFormatter);
            expression.Right.Visit(rightVisitor);

            var left = leftVisitor.ToString();
            var right = rightVisitor.ToString();

            var filterString = $"({left} {expression.Operator} {right})";
            return filterString;
        }
    }
}