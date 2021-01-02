using System.Net.Http;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataCollection<TEntity> : IODataCollection<TEntity> where TEntity : IEntity
    {
        private readonly IValueFormatter _valueFormatter;
        private readonly HttpClient _httpClient;
        private readonly ISerializer _serializer;
        private readonly IPluralizer _pluralizer;

        public ODataCollection(
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

        public IODataQuery<TEntity> Find(ODataFilter<TEntity> filter)
        {
            var query = new ODataQuery<TEntity>(EntityName, _valueFormatter, _httpClient, _serializer, _pluralizer);
            return query.Filter(filter);
        }
    }
}