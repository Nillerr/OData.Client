using System.Collections.Generic;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public static class ODataFindRequestExtensions
    {
        public static string ToQueryString<TEntity>(
            this ODataFindRequest<TEntity> request,
            IValueFormatter valueFormatter
        )
            where TEntity : IEntity
        {
            var parts = new List<string>(3);

            parts.AddFilter(valueFormatter, request.Filter);
            parts.AddSelection(request.Selection);
            parts.AddExpansions(request.Expansions);
            parts.AddSorting(request.Sorting);

            var queryString = string.Join("&", parts);
            return queryString;
        }
    }
}