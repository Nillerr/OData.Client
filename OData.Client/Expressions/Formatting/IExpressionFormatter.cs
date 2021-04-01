namespace OData.Client.Expressions.Formatting
{
    public interface IExpressionFormatter
    {
        string ToString(ODataConstantExpression expression);
        string ToString(ODataFunctionRequestArgument argument);
    }
}