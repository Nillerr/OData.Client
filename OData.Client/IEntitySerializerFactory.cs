namespace OData.Client
{
    public interface IEntitySerializerFactory
    {
        IEntitySerializer<TEntity> CreateSerializer<TEntity>(IEntityName<TEntity> entityName) where TEntity : IEntity;
    }
}