using System.Collections.Generic;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public enum QueryStringFormatting
    {
        None,
        Escaped,
    }
    
    public static class ODataFindRequestExtensions
    {
        public static string ToQueryString<TEntity>(
            this ODataFindRequest<TEntity> request,
            IValueFormatter valueFormatter,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            var parts = new List<string>(3);

            parts.AddFilter(valueFormatter, request.Filter, formatting);
            parts.AddSelection(request.Selection, formatting);
            parts.AddExpansions(request.Expansions, formatting);
            parts.AddSorting(request.Sorting, formatting);

            var queryString = string.Join("&", parts);
            return queryString;
        }

        public static string ToQueryString<TEntity>(
            this ODataRetrieveRequest<TEntity> request,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            var parts = new List<string>(3);

            parts.AddSelection(request.Selection, formatting);
            parts.AddExpansions(request.Expansions, formatting);

            var queryString = string.Join("&", parts);
            return queryString;
        }
    }
}