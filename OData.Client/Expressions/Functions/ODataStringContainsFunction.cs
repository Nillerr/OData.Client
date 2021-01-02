namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringContainsFunction<TEntity> : IODataFunction<TEntity> where TEntity : IEntity
    {
        public static readonly ODataStringContainsFunction<TEntity> Instance = new();

        private ODataStringContainsFunction()
        {
        }

        public string Name { get; } = "contains";
    }
}