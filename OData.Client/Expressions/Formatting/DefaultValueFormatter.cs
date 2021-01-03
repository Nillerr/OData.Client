using System;
using System.Globalization;

namespace OData.Client.Expressions.Formatting
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public string ToString(ODataConstantExpression expression)
        {
            var expressionValue = expression.Value;
            
            return expressionValue switch
            {
                null => "null",
                
                string value => Quoted(value),
                Guid value => Quoted(value, "D"),
                
                byte value => Unquoted(value),
                short value => Unquoted(value),
                int value => Unquoted(value),
                long value => Unquoted(value),
                
                float value => Unquoted(value),
                double value => Unquoted(value),
                decimal value => Unquoted(value),
                
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