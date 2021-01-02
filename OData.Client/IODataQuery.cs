namespace OData.Client
{
    public interface IODataQuery<TEntity> where TEntity : IEntity
    {
        EntityName<TEntity> EntityName { get; }

        IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter);

        IODataQuery<TEntity> Select(params IField<TEntity>[] fields);

        IODataQuery<TEntity> Expand<TOther>(Field<TEntity, TOther?> field) where TOther : IEntity;

        IODataQuery<TEntity> Expand<TOther>(Field<TEntity, TOther?> field, IODataNestedQuery<TOther> query)
            where TOther : IEntity;

        int? MaxPageSize();

        IODataQuery<TEntity> MaxPageSize(int? maxPageSize);

        string ToQueryString();
    }
}