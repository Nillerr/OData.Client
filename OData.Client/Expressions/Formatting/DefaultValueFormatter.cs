using System;
using System.Globalization;

namespace OData.Client.Expressions.Formatting
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public string Serialize(object? expressionValue)
        {
            const string utcDateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'";

            return expressionValue switch
            {
                null => "null",

                string value => Quoted(value),
                Guid value => Quoted(value, "D"),
                IEntityId value => Quoted(value.Id, "D"),

                byte value => Unquoted(value),
                short value => Unquoted(value),
                int value => Unquoted(value),
                long value => Unquoted(value),

                float value => Unquoted(value),
                double value => Unquoted(value),
                decimal value => Unquoted(value),

                bool value => Unquoted(value),

                Enum value => Unquoted(Convert.ToInt32(value)),

                DateTime value => Quoted(value.ToUniversalTime().ToString(utcDateTimeFormat)),
                DateTimeOffset value => Quoted(value.UtcDateTime.ToString(utcDateTimeFormat)),

                IConvertible value => Quoted(value),
                IFormattable value => Quoted(value),

                _ => $"'{expressionValue}'"
            };
        }

        private static string Unquoted<T>(T value) where T : IConvertible
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        private static string Quoted(IConvertible value)
        {
            return $"'{value.ToString(CultureInfo.InvariantCulture)}'";
        }

        private static string Quoted(IFormattable value, string? format = null)
        {
            return $"'{value.ToString(format, CultureInfo.InvariantCulture)}'";
        }
    }
}