namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringEndsWithFunction<TEntity> : IODataFunction<TEntity> where TEntity : IEntity
    {
        public static readonly ODataStringEndsWithFunction<TEntity> Instance = new();

        private ODataStringEndsWithFunction()
        {
        }

        public string Name { get; } = "endswith";
    }
}