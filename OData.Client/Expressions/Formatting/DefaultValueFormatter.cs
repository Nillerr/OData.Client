using System;

namespace OData.Client.Expressions.Formatting
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public string ToString(object? value)
        {
            return value switch
            {
                null => "null",
                string valueAsString => $"'{valueAsString}'",
                Guid guidValue => $"'{guidValue:D}'",
                _ => value.ToString() ?? "null"
            };
        }
    }
}