namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringStartsWithFunction : IODataFunction
    {
        public static readonly ODataStringStartsWithFunction Instance = new();

        public string Name => "startswith";
    }
}