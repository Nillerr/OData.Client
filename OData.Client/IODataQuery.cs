using System.Collections.Generic;

namespace OData.Client
{
    public interface IODataQuery<TEntity> : IAsyncEnumerable<IEntity<TEntity>> where TEntity : IEntity
    {
        IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter);

        IODataQuery<TEntity> Select(IProperty<TEntity> property);
        
        IODataQuery<TEntity> Select(params IProperty<TEntity>[] properties);

        IODataQuery<TEntity> Expand<TOther>(IProperty<TEntity, TOther?> property) where TOther : IEntity;

        // IODataQuery<TEntity> Expand<TOther>(
        //     IProperty<TEntity, TOther?> property,
        //     Func<IODataNestedQuery<TOther>, IODataNestedQuery<TOther>> query
        // )
        //     where TOther : IEntity;

        IODataQuery<TEntity> MaxPageSize(int? maxPageSize);
    }
}