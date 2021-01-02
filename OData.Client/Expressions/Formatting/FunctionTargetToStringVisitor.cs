using System.Text;

namespace OData.Client.Expressions.Formatting
{
    internal sealed class FunctionTargetToStringVisitor<TEntity> : IODataFunctionTargetVisitor<TEntity>
        where TEntity : IEntity
    {
        private readonly StringBuilder _stringBuilder = new();

        public FunctionTargetToStringVisitor(string propertyPrefix)
        {
            PropertyPrefix = propertyPrefix;
        }

        public string PropertyPrefix { get; }

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