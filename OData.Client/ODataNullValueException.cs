namespace OData.Client
{
    public class ODataNullValueException : ODataClientException
    {
        public ODataNullValueException(string? message, IProperty property)
            : base(message)
        {
            Property = property;
        }

        public IProperty Property { get; }
    }
}