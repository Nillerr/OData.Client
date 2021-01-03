namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringEndsWithFunction : ODataFunction
    {
        public static readonly ODataStringEndsWithFunction Instance = new();

        private ODataStringEndsWithFunction()
            : base("endswith")
        {
        }
    }
}