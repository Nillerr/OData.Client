namespace OData.Client
{
    public sealed class ODataFunctionRequestArgument
    {
        public ODataFunctionRequestArgument(object? value)
        {
            Value = value;
        }

        public object? Value { get; }
    }
}