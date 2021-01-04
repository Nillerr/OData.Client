using System.Collections.Generic;

namespace OData.Client
{
    public sealed class ODataFindRequest<TEntity> : IODataFindRequest<TEntity> where TEntity : IEntity
    {
        public ODataFindRequest(
            ODataFilter<TEntity>? filter,
            IEnumerable<IProperty<TEntity>> selection,
            IEnumerable<ODataExpansion<TEntity>> expansions,
            IEnumerable<Sorting<TEntity>> sorting,
            int? maxPageSize
        )
        {
            Filter = filter;
            Selection = selection;
            Expansions = expansions;
            MaxPageSize = maxPageSize;
            Sorting = sorting;
        }

        public ODataFilter<TEntity>? Filter { get; }
        public IEnumerable<IProperty<TEntity>> Selection { get; }
        public IEnumerable<ODataExpansion<TEntity>> Expansions { get; }
        public IEnumerable<Sorting<TEntity>> Sorting { get; }
        public int? MaxPageSize { get; }
    }
    
    
}