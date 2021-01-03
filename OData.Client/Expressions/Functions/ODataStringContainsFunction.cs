namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringContainsFunction : ODataFunction
    {
        public static readonly ODataStringContainsFunction Instance = new();

        private ODataStringContainsFunction()
            : base("contains")
        {
        }
    }
}