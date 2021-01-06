namespace OData.Client
{
    public sealed class ODataPropertiesFactoryContext<TEntity>
        where TEntity : IEntity
    {
        public ODataPropertiesFactoryContext(IODataClient oDataClient, IEntitySetNameResolver entitySetNameResolver, IEntityType<TEntity> entityType)
        {
            ODataClient = oDataClient;
            EntitySetNameResolver = entitySetNameResolver;
            EntityType = entityType;
        }

        public IODataClient ODataClient { get; }
        public IEntitySetNameResolver EntitySetNameResolver { get; }
        public IEntityType<TEntity> EntityType;
    }

    public static class ODataPropertiesFactoryContext
    {
        public static ODataPropertiesFactoryContext<TEntity> Create<TEntity>(
            IODataClient oDataClient,
            IEntitySetNameResolver entitySetNameResolver,
            IEntityType<TEntity> type
        )
            where TEntity : IEntity
        {
            return new(oDataClient, entitySetNameResolver, type);
        }
    }
}