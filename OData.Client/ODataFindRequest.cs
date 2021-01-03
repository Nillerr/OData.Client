using System.Collections.Generic;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public readonly struct ODataFindRequest<TEntity> where TEntity : IEntity
    {
        public ODataFindRequest(
            ODataFilter<TEntity>? filter,
            IEnumerable<IProperty<TEntity>> selection,
            IEnumerable<ODataExpansion<TEntity>> expansions,
            int? maxPageSize
        )
        {
            Filter = filter;
            Selection = selection;
            Expansions = expansions;
            MaxPageSize = maxPageSize;
        }

        public ODataFilter<TEntity>? Filter { get; }
        public IEnumerable<IProperty<TEntity>> Selection { get; }
        public IEnumerable<ODataExpansion<TEntity>> Expansions { get; }
        public int? MaxPageSize { get; }
        
        public string ToQueryString(IValueFormatter valueFormatter)
        {
            var parts = new List<string>(3);

            parts.AddFilter(valueFormatter, Filter);
            parts.AddSelection(Selection);
            parts.AddExpansions(Expansions);

            var queryString = string.Join("&", parts);
            return queryString;
        }
    }
}