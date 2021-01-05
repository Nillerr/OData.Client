namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringContainsFunction : IODataFunction
    {
        public static readonly ODataStringContainsFunction Instance = new();

        public string Name => "contains";
    }
}