using System.Collections.Generic;
using System.Linq;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    internal static class ODataQueryStringBuilder
    {
        public static void AddFilter<TEntity>(
            this List<string> queryStringParts,
            IValueFormatter valueFormatter,
            ODataFilter<TEntity>? filter
        ) where TEntity : IEntity
        {
            if (filter == null) return;

            var oDataFilter = filter.Value;

            var filterVisitor = new FilterExpressionToStringVisitor(string.Empty, valueFormatter);
            oDataFilter.Expression.Visit(filterVisitor);

            var filterString = filterVisitor.ToString();
            queryStringParts.Add("$filter=" + filterString);
        }

        public static void AddExpansions<TEntity>(
            this List<string> queryStringParts,
            IEnumerable<ODataExpansion<TEntity>> expansions
        ) where TEntity : IEntity
        {
            var expandedProperties = expansions.Select(expand => expand.Property.Name).Distinct();
            var expandString = string.Join(",", expandedProperties);
            if (expandString != string.Empty)
            {
                queryStringParts.Add("$expand=" + expandString);
            }
        }

        public static void AddSelection<TEntity>(
            this List<string> queryStringParts,
            IEnumerable<IProperty<TEntity>> selection
        ) where TEntity : IEntity
        {
            var selectedProperties = selection.Select(select => @select.Name).Distinct();
            var selectionString = string.Join(",", selectedProperties);
            if (selectionString != string.Empty)
            {
                queryStringParts.Add("$select=" + selectionString);
            }
        }
    }
}