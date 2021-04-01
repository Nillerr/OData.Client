namespace OData.Client.Expressions.Formatting
{
    internal static class ODataLambdaExpressionExtensions
    {
        public static string ToFilterString(this ODataLambdaExpression expression, IExpressionFormatter expressionFormatter)
        {
            var visitor = new LambdaBodyToStringVisitor("o", expressionFormatter);
            expression.Body.Visit(visitor);

            var body = visitor.ToString();
            
            var filterString = $"{expression.Target.Name}/{expression.Function}(o:{body})";
            return filterString;
        }
    }
}