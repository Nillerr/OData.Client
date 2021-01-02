using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class BinaryRightOperandToStringVisitor<TEntity> : IODataBinaryRightOperandVisitor<TEntity> where TEntity : IEntity
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;
        
        public BinaryRightOperandToStringVisitor(string propertyPrefix, IValueFormatter valueFormatter)
        {
            PropertyPrefix = propertyPrefix;
            _valueFormatter = valueFormatter;
        }

        public string PropertyPrefix { get; }

        public void Visit(ODataConstantExpression<TEntity> expression)
        {
            var stringValue = _valueFormatter.ToString(expression.Value);
            _stringBuilder.Append(stringValue);
        }

        public void Visit(ODataPropertyExpression<TEntity> expression)
        {
            _stringBuilder.Append(PropertyPrefix + expression.Property.Name);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}