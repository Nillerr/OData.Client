using System.Collections.Generic;

namespace OData.Client
{
    internal static class ODataRetrieveRequestExtensions
    {
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