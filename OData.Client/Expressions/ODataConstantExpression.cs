using System;

namespace OData.Client.Expressions
{
    /// <summary>
    /// A constant value expression, e.g. <c>"foo@email.com"</c> or <c>5</c>.
    /// </summary>
    public class ODataConstantExpression : IODataBinaryRightOperand, IODataFunctionArgument
    {
        public ODataConstantExpression(object? value, Type valueType)
        {
            Value = value;
            ValueType = valueType;
        }

        public object? Value { get; }
        
        public Type ValueType { get; }

        public void Visit(IODataBinaryRightOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Visit(IODataFunctionArgumentVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }
}