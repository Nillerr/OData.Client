using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class FunctionTargetToStringVisitor : IODataFunctionTargetVisitor
    {
        private readonly StringBuilder _stringBuilder = new();

        public FunctionTargetToStringVisitor(string propertyPrefix)
        {
            PropertyPrefix = propertyPrefix;
        }

        public string PropertyPrefix { get; }

        public void Visit(ODataPropertyExpression expression)
        {
            _stringBuilder.Append(PropertyPrefix + expression.Property.SelectableName());
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}