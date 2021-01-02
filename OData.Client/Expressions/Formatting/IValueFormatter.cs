namespace OData.Client.Expressions.Formatting
{
    public interface IValueFormatter
    {
        string ToString(object? value);
    }
}