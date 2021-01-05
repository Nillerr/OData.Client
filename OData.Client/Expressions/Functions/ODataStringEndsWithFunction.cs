namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringEndsWithFunction : IODataFunction
    {
        public static readonly ODataStringEndsWithFunction Instance = new();

        public string Name => "endswith";
    }
}