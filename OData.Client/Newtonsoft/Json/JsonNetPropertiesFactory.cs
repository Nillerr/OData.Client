namespace OData.Client.Newtonsoft.Json
{
    public sealed class JsonNetPropertiesFactory : IODataPropertiesFactory
    {
        public IODataProperties<TEntity> Create<TEntity>() where TEntity : IEntity
        {
            return new JObjectProperties<TEntity>();
        }
    }
}