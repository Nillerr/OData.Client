using System.Collections.Generic;
using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal static class ODataFunctionExpressionExtensions
    {
        public static string ToFilterString(
            this ODataFunctionExpression expression,
            string propertyPrefix,
            IValueFormatter valueFormatter
        )
        {
            var visitor = new FunctionArgumentToStringVisitor(valueFormatter, propertyPrefix);
            
            var arguments = expression.Arguments.ToFilterString(visitor);
            var function = expression.Function.Name;

            return $"{function}({arguments})";
        }

        private static string ToFilterString(this IEnumerable<IODataFunctionArgument> arguments, FunctionArgumentToStringVisitor visitor)
        {
            var stringBuilder = new StringBuilder();

            foreach (var argument in arguments)
            {
                argument.Visit(visitor);
                var argumentString = visitor.ToString();

                if (stringBuilder.Length > 0)
                {
                    stringBuilder.EnsureCapacity(argumentString.Length + 1);
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(argumentString);

                visitor.Clear();
            }

            return stringBuilder.ToString();
        }
    }
}