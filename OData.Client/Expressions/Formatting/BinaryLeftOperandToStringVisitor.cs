using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class BinaryLeftOperandToStringVisitor : IODataBinaryLeftOperandVisitor
    {
        private readonly StringBuilder _stringBuilder = new();
        
        public BinaryLeftOperandToStringVisitor(string propertyPrefix)
        {
            PropertyPrefix = propertyPrefix;
        }

        private string PropertyPrefix { get; }
        
        public void Visit(ODataPropertyExpression expression)
        {
            _stringBuilder.Append(PropertyPrefix + expression.Property.SelectableName);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}