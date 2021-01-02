namespace OData.Client.Expressions.Functions
{
    public sealed class ODataStringStartsWithFunction<TEntity> : IODataFunction<TEntity> where TEntity : IEntity
    {
        public static readonly ODataStringStartsWithFunction<TEntity> Instance = new();

        private ODataStringStartsWithFunction()
        {
        }

        public string Name { get; } = "startswith";
    }
}