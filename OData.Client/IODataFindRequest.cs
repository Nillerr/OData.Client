using System.Collections.Generic;

namespace OData.Client
{
    public interface IODataFindRequest<TEntity> where TEntity : IEntity
    {
        ODataFilter<TEntity>? Filter { get; }
        IEnumerable<IProperty<TEntity>> Selection { get; }
        IEnumerable<ODataExpansion<TEntity>> Expansions { get; }
        IEnumerable<Sorting<TEntity>> Sorting { get; }
        int? MaxPageSize { get; }
    }
}