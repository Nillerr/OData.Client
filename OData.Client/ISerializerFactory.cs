namespace OData.Client
{
    public interface ISerializerFactory
    {
        ISerializer<TEntity> CreateSerializer<TEntity>(IEntityName<TEntity> entityName) where TEntity : IEntity;
    }
}