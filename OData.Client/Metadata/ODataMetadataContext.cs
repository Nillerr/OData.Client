namespace OData.Client
{
    public sealed class ODataMetadataContext<TEntity>
        where TEntity : IEntity
    {
        public ODataMetadataContext(IODataClient oDataClient, IEntityType<TEntity> entityType)
        {
            ODataClient = oDataClient;
            EntityType = entityType;
        }

        public IODataClient ODataClient { get; }
        public IEntityType<TEntity> EntityType { get; }
    }

    public static class ODataMetadataContext
    {
        public static ODataMetadataContext<TEntity> Create<TEntity>(
            IODataClient oDataClient,
            IEntityType<TEntity> entityType
        )
            where TEntity : IEntity
        {
            return new ODataMetadataContext<TEntity>(oDataClient, entityType);
        }
    }
}