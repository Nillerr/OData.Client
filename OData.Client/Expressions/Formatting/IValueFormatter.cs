namespace OData.Client.Expressions.Formatting
{
    public interface IValueFormatter
    {
        string ToString(ODataConstantExpression expression);
        string ToString(ODataFunctionRequestArgument argument);
    }
}