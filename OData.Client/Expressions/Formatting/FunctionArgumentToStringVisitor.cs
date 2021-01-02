using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class FunctionArgumentToStringVisitor<TEntity> : IODataFunctionArgumentVisitor<TEntity>
        where TEntity : IEntity
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly IValueFormatter _valueFormatter;

        public FunctionArgumentToStringVisitor(IValueFormatter valueFormatter)
        {
            _valueFormatter = valueFormatter;
        }

        public void Visit(ODataConstantExpression<TEntity> expression)
        {
            var stringValue = _valueFormatter.ToString(expression.Value);
            _stringBuilder.Append(stringValue);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}