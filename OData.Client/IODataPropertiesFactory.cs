namespace OData.Client
{
    public interface IODataPropertiesFactory
    {
        IODataProperties<TEntity> Create<TEntity>() where TEntity : IEntity;
    }
}