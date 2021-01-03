namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringStartsWithFunction : ODataFunction
    {
        public static readonly ODataStringStartsWithFunction Instance = new();

        private ODataStringStartsWithFunction()
            : base("startswith")
        {
        }
    }
}