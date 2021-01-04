namespace OData.Client
{
    public interface IEntityName
    {
        string Name { get; }
    }
    
    public interface IEntityName<out TEntity> : IEntityName 
        where TEntity : IEntity
    {
    }
}