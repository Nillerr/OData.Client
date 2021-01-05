namespace OData.Client
{
    public sealed class ODataFindNextRequest<TEntity> : IODataFindRequestHeaders<TEntity>
        where TEntity : IEntity
    {
        public ODataFindNextRequest(int? maxPageSize)
        {
            MaxPageSize = maxPageSize;
        }

        public int? MaxPageSize { get; }
    }
}