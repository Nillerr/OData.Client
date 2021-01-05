using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    /// <summary>
    /// An OData client implementation for interacting with an OData API.
    /// </summary>
    public sealed class ODataClient : IODataClient
    {
        private readonly Uri _organizationUri;
        private readonly IODataPropertiesFactory _propertiesFactory;
        private readonly IEntitySerializer _entitySerializer;
        private readonly IODataHttpClient _oDataHttpClient;
        private readonly IValueFormatter _valueFormatter;
        private readonly IEntitySetNameResolver _entitySetNameResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataClient"/> with the specified <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">The client settings.</param>
        public ODataClient(ODataClientSettings settings)
        {
            _organizationUri = settings.OrganizationUri;
            _propertiesFactory = settings.PropertiesFactory;
            _entitySerializer = settings.EntitySerializer;
            _oDataHttpClient = settings.HttpClient;
            _valueFormatter = settings.ValueFormatter;
            _entitySetNameResolver = settings.EntitySetNameResolver;
        }

        private Uri BaseUri => new Uri(_organizationUri, "api/data/v9.1/");

        private Uri CollectionUri<T>(IEntityType<T> type) where T : IEntity => new Uri(BaseUri, EntitySetName(type));

        private string EntitySetName<T>(IEntityType<T> type) where T : IEntity => _entitySetNameResolver.EntitySetName(type);

        private Uri EntityUri<T>(IEntityId<T> id) where T : IEntity => new Uri(BaseUri, $"{EntitySetName(id.Type)}({id.Id:D})");

        private Uri FindUri<TEntity>(IEntityType<TEntity> type, ODataFindRequest<TEntity> request)
            where TEntity : IEntity
        {
            var collectionUri = CollectionUri(type);

            var requestUriBuilder = new UriBuilder(collectionUri);
            requestUriBuilder.Query = request.ToQueryString(_valueFormatter, QueryStringFormatting.UrlEscaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private Uri RetrieveUri<TEntity>(IEntityId<TEntity> id, ODataRetrieveRequest<TEntity> request)
            where TEntity : IEntity
        {
            var entityUri = EntityUri(id);

            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Query = request.ToQueryString(QueryStringFormatting.UrlEscaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private Uri PropertyRefUri<TEntity, TOther>(IEntityId<TEntity> id, IRefProperty<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var entityUri = EntityUri(id);

            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Path += $"/{property.SelectableName}/$ref";

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        /// <inheritdoc />
        public IODataCollection<TEntity> Collection<TEntity>(IEntityType<TEntity> type) where TEntity : IEntity
        {
            return new ODataCollection<TEntity>(type, this, _valueFormatter);
        }

        /// <inheritdoc />
        public async Task<IFindResponse<TEntity>> FindAsync<TEntity>(
            IEntityType<TEntity> type,
            ODataFindRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = FindUri(type, request);
                var httpRequest = CreateFindRequest(HttpMethod.Get, requestUri, request);
                return ValueTask.FromResult(httpRequest);
            }
            
            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var response = await _entitySerializer.DeserializeFindResponseAsync(stream, type);
            return response;
        }

        /// <inheritdoc />
        public async Task<IFindResponse<TEntity>?> FindNextAsync<TEntity>(
            IFindResponse<TEntity> current,
            ODataFindNextRequest<TEntity> request,
            CancellationToken cancellationToken = default
        ) where TEntity : IEntity
        {
            var requestUri = current.NextLink;
            if (requestUri == null)
            {
                return null;
            }
            
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var httpRequest = CreateFindRequest(HttpMethod.Get, requestUri, request);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var response = await _entitySerializer.DeserializeFindResponseAsync(stream, current.EntityType);
            return response;
        }

        private static HttpRequestMessage CreateFindRequest<TEntity>(
            HttpMethod method,
            Uri requestUri,
            IODataFindRequestHeaders<TEntity> headers
        )
            where TEntity : IEntity
        {
            var httpRequest = CreateDefaultRequest(method, requestUri);
            
            if (headers.MaxPageSize.HasValue)
            {
                httpRequest.Headers.Add("Prefer", $"odata.maxpagesize={headers.MaxPageSize.Value}");
            }
            
            return httpRequest;
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>?> RetrieveAsync<TEntity>(
            IEntityId<TEntity> id,
            ODataRetrieveRequest<TEntity> request,
            CancellationToken cancellationToken = default
        ) 
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = RetrieveUri(id, request);
                var httpRequest = CreateDefaultRequest(HttpMethod.Get, requestUri);
                return ValueTask.FromResult(httpRequest);
            }

            var httpRequestOptions = new ODataHttpRequestOptions();
            httpRequestOptions.AllowedStatusCodes.Add(HttpStatusCode.NotFound);
            
            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, httpRequestOptions, cancellationToken);
            
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var entity = await httpResponse.Content.ReadEntityAsync(id.Type, _entitySerializer, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task<EntityId<TEntity>> CreateAsync<TEntity>(
            IEntityType<TEntity> type,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = CollectionUri(type);
                var httpRequest = CreatePropsRequest(HttpMethod.Post, requestUri, props);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);

            var entityId = httpResponse.Headers.EntityId(type);
            return entityId;
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>> CreateRepresentationAsync<TEntity>(
            IEntityType<TEntity> type,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = CollectionUri(type);
                var httpRequest = CreatePropsRepresentationRequest(HttpMethod.Post, requestUri, props);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            var entity = await httpResponse.Content.ReadEntityAsync(type, _entitySerializer, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task UpdateAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = EntityUri(id);
                var httpRequest = CreatePropsRequest(HttpMethod.Patch, requestUri, props);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>> UpdateRepresentationAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = EntityUri(id);
                var httpRequest = CreatePropsRepresentationRequest(HttpMethod.Patch, requestUri, props);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            var entity = await httpResponse.Content.ReadEntityAsync(id.Type, _entitySerializer, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task DeleteAsync<TEntity>(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
            where TEntity : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var httpRequest = CreateEmptyRequest(HttpMethod.Delete, EntityUri(id));
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var httpRequest = CreatePropsRequest<Ref>(HttpMethod.Put, PropertyRefUri(id, property), props =>
                {
                    props.Set(Ref.Id, EntityUri(otherId));
                });
                
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var httpRequest = CreatePropsRequest<Ref>(HttpMethod.Put, PropertyRefUri(id, property), props =>
                {
                    props.Set(Ref.Id, EntityUri(otherId));
                });
                
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var requestUri = PropertyRefUri(id, property);
                var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            ValueTask<HttpRequestMessage> CreateRequest()
            {
                var propertyRefUri = PropertyRefUri(id, property);
                var otherEntityUri = EntityUri(otherId);

                var requestUriBuilder = new UriBuilder(propertyRefUri);
                requestUriBuilder.Query = $"$id={otherEntityUri}";

                var requestUri = requestUriBuilder.Uri;
                var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
                return ValueTask.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        private HttpRequestMessage CreatePropsRequest<T>(
            HttpMethod method,
            Uri requestUri,
            Action<IODataProperties<T>> props
        )
            where T : IEntity
        {
            var properties = _propertiesFactory.Create<T>();
            props(properties);

            var httpContent = properties.ToHttpContent();
            return CreateMutationRequest(method, requestUri, httpContent);
        }

        private HttpRequestMessage CreatePropsRepresentationRequest<T>(
            HttpMethod method,
            Uri requestUri,
            Action<IODataProperties<T>> props
        )
            where T : IEntity
        {
            var httpRequest = CreatePropsRequest(method, requestUri, props);
            httpRequest.Headers.Add("Prefer", "return=representation");
            return httpRequest;
        }

        private static HttpRequestMessage CreateEmptyRequest(HttpMethod httpMethod, Uri requestUri)
        {
            var httpRequest = CreateDefaultRequest(httpMethod, requestUri);
            httpRequest.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
            return httpRequest;
        }

        private static HttpRequestMessage CreateMutationRequest(HttpMethod method, Uri requestUri, HttpContent content)
        {
            var httpRequest = CreateDefaultRequest(method, requestUri);
            httpRequest.Content = content;
            return httpRequest;
        }

        private static HttpRequestMessage CreateDefaultRequest(HttpMethod method, Uri requestUri)
        {
            var httpRequest = new HttpRequestMessage(method, requestUri);
            httpRequest.Headers.Accept.ParseAdd(MediaTypeNames.Application.Json);
            httpRequest.Headers.Add("OData-MaxVersion", "4.0");
            httpRequest.Headers.Add("OData-Version", "4.0");
            return httpRequest;
        }
    }
}