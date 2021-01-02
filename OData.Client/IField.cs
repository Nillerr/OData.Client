namespace OData.Client
{
    public interface IField<TEntity>
        where TEntity : IEntity
    {
        string Name { get; }
    }
}