using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataCollection<TEntity> : IODataCollection<TEntity> where TEntity : IEntity
    {
        private readonly IValueFormatter _valueFormatter;
        private readonly HttpClient _httpClient;
        private readonly ISerializer _serializer;
        private readonly IPluralizer _pluralizer;
        private readonly IODataPropertiesFactory _propertiesFactory;
            
        private readonly Uri _organizationUri = new Uri("https://universal-robots-uat.crm4.dynamics.com/");

        public ODataCollection(
            EntityName<TEntity> entityName,
            IValueFormatter valueFormatter,
            HttpClient httpClient,
            ISerializer serializer,
            IPluralizer pluralizer,
            IODataPropertiesFactory propertiesFactory
        )
        {
            EntityName = entityName;
            _valueFormatter = valueFormatter;
            _httpClient = httpClient;
            _serializer = serializer;
            _pluralizer = pluralizer;
            _propertiesFactory = propertiesFactory;
        }

        public EntityName<TEntity> EntityName { get; }

        public IODataQuery<TEntity> Find()
        {
            return new ODataQuery<TEntity>(EntityName, _valueFormatter, _httpClient, _serializer, _pluralizer);
        }

        public async Task<Guid> CreateAsync(
            Action<IODataProperties<TEntity>> set,
            CancellationToken cancellationToken = default
        )
        {
            var baseUri = new Uri(_organizationUri, "api/data/v9.1/");

            var pluralEntityName = _pluralizer.ToPlural(EntityName.Name);
            var requestUri = new Uri(baseUri, $"{pluralEntityName}/");
            
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            
            httpRequest.Headers.Accept.ParseAdd(MediaTypeNames.Application.Json);
            httpRequest.Headers.Add("OData-MaxVersion", "4.0");
            httpRequest.Headers.Add("OData-Version", "4.0");

            var properties = _propertiesFactory.Create<TEntity>();
            set(properties);

            httpRequest.Content = properties.ToHttpContent();
            
            using var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException exception)
            {
                var message = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(message, exception, exception.StatusCode);
            }

            var entityId = EntityId(httpResponse);
            return entityId;
        }

        /// <summary>
        /// Extracts the <see cref="Guid"/> from the <c>OData-EntityId</c> header value.
        /// </summary>
        /// <remarks>
        /// The <c>OData-EntityId</c> header value has the format
        /// <c>OData-EntityId: [Organization URI]/api/data/v9.0/contacts(6124b4e0-9299-483d-9cf2-5886533215e7)</c>, and
        /// as such the <see cref="Guid"/> stored within must be extracted from the rest of the "id", as the remaining
        /// part is of little value.
        ///
        /// TODO @nije: Revise the wording and consider introducing an EntityId type holding the type of entity as well as the Guid
        /// </remarks>
        /// <param name="httpResponse">The HTTP response</param>
        /// <returns>The id portion of the <c>OData-EntityId</c> header value.</returns>
        private static Guid EntityId(HttpResponseMessage httpResponse)
        {
            var entityIdValues = httpResponse.Headers.GetValues("OData-EntityId");
            var entityIdValue = entityIdValues.Single();

            const int guidLength = 36;
            var entityIdString = entityIdValue.Substring(entityIdValue.Length - guidLength - 1, guidLength);
            
            var entityId = Guid.Parse(entityIdString);
            return entityId;
        }

        public async Task DeleteAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
        {
            var baseUri = new Uri(_organizationUri, "api/data/v9.1/");

            var pluralEntityName = _pluralizer.ToPlural(EntityName.Name);
            var requestUri = new Uri(baseUri, $"{pluralEntityName}/({id})");
            
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            
            httpRequest.Headers.Add("OData-MaxVersion", "4.0");
            httpRequest.Headers.Add("OData-Version", "4.0");
            httpRequest.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
            
            using var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException exception)
            {
                var message = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(message, exception, exception.StatusCode);
            }
        }
    }
}