namespace OData.Client
{
    public interface IEntityName<out TEntity> where TEntity : IEntity
    {
        string Name { get; }
    }
}