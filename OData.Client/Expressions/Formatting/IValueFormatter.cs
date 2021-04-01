namespace OData.Client.Expressions.Formatting
{
    public interface IValueFormatter
    {
        string Serialize(object? expressionValue);
    }
}