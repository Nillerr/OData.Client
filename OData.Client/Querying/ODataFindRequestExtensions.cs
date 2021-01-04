using System.Collections.Generic;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    /// <summary>
    /// Extension methods on <see cref="ODataFindRequest{TEntity}"/>.
    /// </summary>
    public static class ODataFindRequestExtensions
    {
        /// <summary>
        /// Returns a query string representation of the request using the specified value formatter and formatting.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="valueFormatter">The value formatter.</param>
        /// <param name="formatting">The formatting to apply to the query string.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The query string representation of the request.</returns>
        public static string ToQueryString<TEntity>(
            this ODataFindRequest<TEntity> request,
            IValueFormatter valueFormatter,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            var parts = new List<string>(4);

            parts.AddFilter(valueFormatter, request.Filter, formatting);
            parts.AddSelection(request.Selection, formatting);
            parts.AddExpansions(request.Expansions, formatting);
            parts.AddSorting(request.Sorting, formatting);

            var queryString = string.Join("&", parts);
            return queryString;
        }

        /// <summary>
        /// Returns a query string representation of the request using the specified formatting.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="formatting">The formatting to apply to the query string.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The query string representation of the request.</returns>
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