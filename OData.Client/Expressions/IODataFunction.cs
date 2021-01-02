namespace OData.Client.Expressions
{
    public interface IODataFunction<TEntity> where TEntity : IEntity
    {
        string Name { get; }
    }
}