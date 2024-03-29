using System;
using System.Collections.Generic;
using System.Linq;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    internal static class ODataQueryStringBuilder
    {
        public static void AddFilter<TEntity>(
            this List<string> queryStringParts,
            IExpressionFormatter expressionFormatter,
            ODataFilter<TEntity>? filter,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            if (filter == null)
            {
                return;
            }

            var oDataFilter = filter.Value;
            
            var filterExpression = oDataFilter.Expression;
            if (filterExpression == null)
            {
                return;
            }

            var filterVisitor = new FilterExpressionToStringVisitor(string.Empty, expressionFormatter);
            filterExpression.Visit(filterVisitor);

            var filterString = filterVisitor.ToString();
            
            var valuePart = ValuePart(filterString, formatting);
            queryStringParts.Add("$filter=" + valuePart);
        }

        public static void AddExpansions<TEntity>(
            this List<string> queryStringParts,
            IEnumerable<ODataExpansion<TEntity>> expansions,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            var expandedProperties = expansions.Select(expand => expand.Property.ExpandableName).Distinct();
            var expandString = string.Join(",", expandedProperties);
            if (expandString != string.Empty)
            {
                var valuePart = ValuePart(expandString, formatting);
                queryStringParts.Add("$expand=" + valuePart);
            }
        }

        public static void AddSelection<TEntity>(
            this List<string> queryStringParts,
            IEnumerable<ISelectableProperty<TEntity>> selection,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            var selectedProperties = selection.Select(select => select.SelectableName).Distinct();
            var selectionString = string.Join(",", selectedProperties);
            if (selectionString != string.Empty)
            {
                var valuePart = ValuePart(selectionString, formatting);
                queryStringParts.Add("$select=" + valuePart);
            }
        }

        public static void AddSorting<TEntity>(
            this List<string> queryStringParts,
            IEnumerable<Sorting<TEntity>> sorting,
            QueryStringFormatting formatting
        )
            where TEntity : IEntity
        {
            var selectedSorting = sorting
                .Select(sort => $"{sort.Property.SortableName} {sort.Direction.ToQueryPart()}");
            
            var orderByString = string.Join(",", selectedSorting);
            if (orderByString != string.Empty)
            {
                var valuePart = ValuePart(orderByString, formatting);
                queryStringParts.Add("$orderby=" + valuePart);
            }
        }

        private static string ToQueryPart(this SortDirection direction) => direction switch
        {
            SortDirection.Ascending => "asc",
            SortDirection.Descending => "desc",
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        private static string ValuePart(string value, QueryStringFormatting formatting)
        {
            var valuePart = formatting == QueryStringFormatting.UrlEscaped ? Uri.EscapeDataString(value) : value;
            return valuePart;
        }
    }
}