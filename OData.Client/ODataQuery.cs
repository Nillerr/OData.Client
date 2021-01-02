using System.Collections.Generic;
using System.Linq;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataQuery<TEntity> : IODataQuery<TEntity> where TEntity : IEntity
    {
        private readonly List<string> _expansions = new();

        private string _filter = string.Empty;
        private string _selection = string.Empty;

        private int? _maxPageSize;
        
        private readonly IValueFormatter _valueFormatter;

        public ODataQuery(EntityName<TEntity> entityName, IValueFormatter valueFormatter)
        {
            EntityName = entityName;
            _valueFormatter = valueFormatter;
        }

        public EntityName<TEntity> EntityName { get; }

        public IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter)
        {
            var filterVisitor = new FilterExpressionToStringVisitor<TEntity>(string.Empty, _valueFormatter);
            filter.Expression.Visit(filterVisitor);
            
            _filter = filterVisitor.ToString();
            return this;
        }

        public IODataQuery<TEntity> Select(params IProperty<TEntity>[] fields)
        {
            _selection = string.Join(",", fields.Select(field => field.Name));
            return this;
        }

        public IODataQuery<TEntity> Expand<TOther>(Property<TEntity, TOther?> property) where TOther : IEntity
        {
            _expansions.Add(property.Name);
            return this;
        }

        public IODataQuery<TEntity> Expand<TOther>(Property<TEntity, TOther?> property, IODataNestedQuery<TOther> query)
            where TOther : IEntity
        {
            _expansions.Add($"{property.Name}({query.ToQueryString()})");
            return this;
        }

        public int? MaxPageSize()
        {
            return _maxPageSize;
        }

        public IODataQuery<TEntity> MaxPageSize(int? maxPageSize)
        {
            _maxPageSize = maxPageSize;
            return this;
        }

        public string ToQueryString()
        {
            var queryStringParts = new List<string>(3);

            if (_filter != string.Empty)
            {
                queryStringParts.Add("$filter=" + _filter);
            }

            if (_selection != string.Empty)
            {
                queryStringParts.Add("$select=" + _selection);
            }

            if (_expansions.Count > 0)
            {
                var expand = string.Join(",", _expansions);
                queryStringParts.Add("$expand=" + expand);
            }

            var queryString = string.Join("&", queryStringParts);
            return queryString;
        }
    }
}