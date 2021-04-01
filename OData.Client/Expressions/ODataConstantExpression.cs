using System;

namespace OData.Client.Expressions
{
    /// <summary>
    /// A constant value expression, e.g. <c>"foo@email.com"</c> or <c>5</c>.
    /// </summary>
    [Equals]
    public class ODataConstantExpression : IODataBinaryRightOperand, IODataFunctionArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataConstantExpression"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueType">The value type.</param>
        private ODataConstantExpression(object? value, Type valueType)
        {
            Value = value;
            ValueType = valueType;
        }

        /// <summary>
        /// An expression that evaluates to <see langword="null"/>.
        /// </summary>
        public static readonly ODataConstantExpression Null = new ODataConstantExpression(null, typeof(object));

        /// <summary>
        /// Creates a new instance of the <see cref="ODataConstantExpression"/> class holding the specified
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property to create a value for.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <returns>A constant expression holding the value.</returns>
        public static ODataConstantExpression Create<TEntity, TValue>(IProperty<TEntity, TValue> property, TValue value)
            where TEntity : IEntity
        {
            return value is null ? Null : new ODataConstantExpression(value, typeof(TValue?));
        }

        /// <summary>
        /// The value.
        /// </summary>
        public object? Value { get; }
        
        /// <summary>
        /// The value type.
        /// </summary>
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
            return Value?.ToString() ?? "null";
        }
    }
}