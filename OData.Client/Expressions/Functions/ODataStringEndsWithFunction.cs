namespace OData.Client.Expressions.Functions
{
    [Equals]
    public sealed class ODataStringEndsWithFunction : IODataFunction
    {
        public static readonly ODataStringEndsWithFunction Instance = new();

        public string Name => "endswith";
    }
}