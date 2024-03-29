namespace OData.Client
{
    public interface IODataNestedQuery<TEntity> where TEntity : IEntity
    {
        IODataNestedQuery<TEntity> Filter(ODataFilter<TEntity> filter);

        IODataNestedQuery<TEntity> Select(params IProperty<TEntity>[] fields);

        string ToQueryString();
    }
}