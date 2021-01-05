namespace OData.Client.Expressions
{
    /// <summary>
    /// Represents a lambda expression, used for `{field}/{function}(o:{body})`
    /// </summary>
    public class ODataLambdaExpression : IODataFilterExpression, IODataLogicalOperand
    {
        public ODataLambdaExpression(IRefs property, string function, IODataLambdaBody body)
        {
            Function = function;
            Body = body;
            Property = property;
        }

        public IRefs Property;

        public string Function { get; }

        public IODataLambdaBody Body { get; }

        public void Visit(IODataLogicalOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFilterExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Property)}: {Property}, {nameof(Function)}: {Function}, {nameof(Body)}: {Body}";
        }
    }
}