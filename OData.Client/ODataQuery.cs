using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
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

        private readonly Uri _organizationUri = new Uri("https://universal-robots-uat.crm4.dynamics.com/");
        private readonly HttpClient _httpClient;
        private readonly ISerializer _serializer;
        private readonly IPluralizer _pluralizer;

        public ODataQuery(
            EntityName<TEntity> entityName,
            IValueFormatter valueFormatter,
            HttpClient httpClient,
            ISerializer serializer,
            IPluralizer pluralizer
        )
        {
            EntityName = entityName;
            _valueFormatter = valueFormatter;
            _httpClient = httpClient;
            _serializer = serializer;
            _pluralizer = pluralizer;
        }

        public EntityName<TEntity> EntityName { get; }

        public IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter)
        {
            var filterVisitor = new FilterExpressionToStringVisitor<TEntity>(string.Empty, _valueFormatter);
            filter.Expression.Visit(filterVisitor);
            
            _filter = filterVisitor.ToString();
            return this;
        }

        public IODataQuery<TEntity> Select(params IProperty<TEntity>[] properties)
        {
            _selection = string.Join(",", properties.Select(PropertySelectName));
            return this;
        }

        private static string PropertySelectName(IProperty<TEntity> property)
        {
            if (property.ValueType.IsAssignableTo(typeof(IEntity)))
            {
                return $"_{property.Name}_value";
            }

            if (property.ValueType.IsEnumerableType(out var valueType))
            {
                return property.Name;
            }

            return property.Name;
        }

        public IODataQuery<TEntity> Expand<TOther>(Property<TEntity, TOther?> property) where TOther : IEntity
        {
            _expansions.Add(property.Name);
            return this;
        }

        public IODataQuery<TEntity> Expand<TOther>(
            Property<TEntity, TOther?> property,
            Func<IODataNestedQuery<TOther>, IODataNestedQuery<TOther>> query
        )
            where TOther : IEntity
        {
            throw new NotImplementedException();
            // _expansions.Add($"{property.Name}({query.ToQueryString()})");
            // return this;
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

        public async IAsyncEnumerator<IEntity<TEntity>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var baseUri = new Uri(_organizationUri, "api/data/v9.1/");
            
            var pluralEntityName = _pluralizer.ToPlural(EntityName.Name);
            var entityUri = new Uri(baseUri, $"{pluralEntityName}/");
            
            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Query = ToQueryString();
            
            var requestUri = requestUriBuilder.Uri;
            
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httpRequest.Headers.Accept.ParseAdd(MediaTypeNames.Application.Json);
            httpRequest.Headers.Add("OData-MaxVersion", "4.0");
            httpRequest.Headers.Add("OData-Version", "4.0");
            
            if (_maxPageSize.HasValue)
            {
                httpRequest.Headers.Add("Prefer", $"odata.maxpagesize={_maxPageSize.Value}");
            }

            using var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                var message = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(message, null, httpResponse.StatusCode);
            }
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
            
            var response = await _serializer.DeserializeFindResponseAsync<TEntity>(stream);
            foreach (var entity in response.Value)
            {
                yield return entity;
            }
        }
    }
}