using System;
using System.Collections.Generic;

namespace OData.Client
{
    public interface IODataQuery<TEntity> : IAsyncEnumerable<IEntity<TEntity>> where TEntity : IEntity
    {
        EntityName<TEntity> EntityName { get; }

        IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter);

        IODataQuery<TEntity> Select(params IProperty<TEntity>[] properties);

        IODataQuery<TEntity> Expand<TOther>(Property<TEntity, TOther?> property) where TOther : IEntity;

        IODataQuery<TEntity> Expand<TOther>(
            Property<TEntity, TOther?> property,
            Func<IODataNestedQuery<TOther>, IODataNestedQuery<TOther>> query
        )
            where TOther : IEntity;

        int? MaxPageSize();

        IODataQuery<TEntity> MaxPageSize(int? maxPageSize);

        string ToQueryString();
    }
}