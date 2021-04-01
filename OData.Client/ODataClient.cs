using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IEntitySetNameResolver _entitySetNameResolver;
        private readonly ILogger<ODataClient> _logger;

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
            _expressionFormatter = settings.ExpressionFormatter;
            _entitySetNameResolver = settings.EntitySetNameResolver;
            _logger = settings.LoggerFactory.CreateLogger<ODataClient>();
        }

        private Uri BaseUri => new Uri(_organizationUri, "api/data/v9.1/");

        private async Task<Uri> CollectionUriASync<TEntity>(IEntityType<TEntity> type)
            where TEntity : IEntity
        {
            var entitySetName = await EntitySetNameAsync(type);
            return new Uri(BaseUri, entitySetName);
        }

        private async Task<string> EntitySetNameAsync<TEntity>(IEntityType<TEntity> type)
            where TEntity : IEntity
        {
            var context = new ODataMetadataContext<TEntity>(this, type);
            var entitySetName = await _entitySetNameResolver.EntitySetNameAsync(context);
            return entitySetName;
        }

        private async Task<Uri> EntityUriAsync<TEntity>(IEntityId<TEntity> id)
            where TEntity : IEntity
        {
            var entitySetName = await EntitySetNameAsync(id.Type);
            return new Uri(BaseUri, $"{entitySetName}({id.Id:D})");
        }

        private async Task<Uri> FindUriAsync<TEntity>(IEntityType<TEntity> type, ODataFindRequest<TEntity> request)
            where TEntity : IEntity
        {
            var collectionUri = await CollectionUriASync(type);

            var requestUriBuilder = new UriBuilder(collectionUri);
            requestUriBuilder.Query = request.ToQueryString(_expressionFormatter, QueryStringFormatting.UrlEscaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private async Task<Uri> RetrieveUriAsync<TEntity>(IEntityId<TEntity> id, ODataRetrieveRequest<TEntity> request)
            where TEntity : IEntity
        {
            var entityUri = await EntityUriAsync(id);

            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Query = request.ToQueryString(QueryStringFormatting.UrlEscaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private async Task<Uri> PropertyRefUriAsync<TEntity, TOther>(IEntityId<TEntity> id, IRefProperty<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var entityUri = await EntityUriAsync(id);

            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Path += $"/{property.SelectableName}/$ref";

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private Uri FunctionUri<TResult>(ODataFunctionRequest<TResult> request)
            where TResult : IEntity
        {
            var argumentPairs = request.Arguments.Select(parameter => $"{parameter.Key}={_expressionFormatter.ToString(parameter.Value)}");
            var argumentsPart = string.Join(",", argumentPairs);
            var functionUri = new Uri(BaseUri, $"{request.FunctionName}({argumentsPart})");
            
            var requestUriBuilder = new UriBuilder(functionUri);
            requestUriBuilder.Query = request.ToQueryString(QueryStringFormatting.UrlEscaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        /// <inheritdoc />
        public IODataCollection<TEntity> Collection<TEntity>(IEntityType<TEntity> type) where TEntity : IEntity
        {
            return new ODataCollection<TEntity>(type, this, _expressionFormatter);
        }

        /// <inheritdoc />
        public async Task<IFindResponse<TEntity>> FindAsync<TEntity>(
            IEntityType<TEntity> type,
            ODataFindRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var queryString = request.ToQueryString(_expressionFormatter, QueryStringFormatting.None);
                _logger.LogDebug("Querying entities of type '{EntityType}' using request \"{Request}\"...", type.Name, queryString);
            }
            
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await FindUriAsync(type, request);
                var httpRequest = CreateFindRequest(HttpMethod.Get, requestUri, request);
                return httpRequest;
            }
            
            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var response = await _entitySerializer.DeserializeFindResponseAsync(stream, type);
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                // TODO @nije: Add ToJson() for response
                var queryString = request.ToQueryString(_expressionFormatter, QueryStringFormatting.None);
                _logger.LogDebug("Query for entities of type '{EntityType}' using request \"{Request}\" returned {Count} results", type.Name, queryString, response.Value.Count);
            }
            
            return response;
        }

        /// <inheritdoc />
        public async Task<IFindResponse<TEntity>?> FindNextAsync<TEntity>(
            IFindResponse<TEntity> current,
            ODataFindNextRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var requestUri = current.NextLink;
            if (requestUri == null)
            {
                return null;
            }
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Querying the next page of entities of type '{EntityType}' using request \"{RequestUri}\"...", current.EntityType.Name, requestUri);
            }
            
            Task<HttpRequestMessage> CreateRequest()
            {
                var httpRequest = CreateFindRequest(HttpMethod.Get, requestUri, request);
                return Task.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var response = await _entitySerializer.DeserializeFindResponseAsync(stream, current.EntityType);
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Query for the next page of entities of type '{EntityType}' using request \"{RequestUri}\" returned {Count} results", current.EntityType.Name, requestUri, response.Value.Count);
            }
            
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await RetrieveUriAsync(id, request);
                var httpRequest = CreateDefaultRequest(HttpMethod.Get, requestUri);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await CollectionUriASync(type);
                var httpRequest = await CreatePropsRequestAsync(type, HttpMethod.Post, requestUri, props);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await CollectionUriASync(type);
                var httpRequest = await CreatePropsRepresentationRequestAsync(type, HttpMethod.Post, requestUri, props);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await EntityUriAsync(id);
                var httpRequest = await CreatePropsRequestAsync(id.Type, HttpMethod.Patch, requestUri, props);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await EntityUriAsync(id);
                var httpRequest = await CreatePropsRepresentationRequestAsync(id.Type, HttpMethod.Patch, requestUri, props);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await EntityUriAsync(id);
                var httpRequest = CreateEmptyRequest(HttpMethod.Delete, requestUri);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await PropertyRefUriAsync(id, property);
                var entityUri = await EntityUriAsync(otherId);
                
                var httpRequest = await CreatePropsRequestAsync(Ref.EntityType, HttpMethod.Put, requestUri, props =>
                {
                    props.Set(Ref.Id, entityUri);
                });
                
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await PropertyRefUriAsync(id, property);
                var entityUri = await EntityUriAsync(otherId);
                
                var httpRequest = await CreatePropsRequestAsync(Ref.EntityType, HttpMethod.Put, requestUri, props =>
                {
                    props.Set(Ref.Id, entityUri);
                });
                
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = await PropertyRefUriAsync(id, property);
                var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
                return httpRequest;
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
            async Task<HttpRequestMessage> CreateRequest()
            {
                var propertyRefUri = await PropertyRefUriAsync(id, property);
                var otherEntityUri = await EntityUriAsync(otherId);

                var requestUriBuilder = new UriBuilder(propertyRefUri);
                requestUriBuilder.Query = $"$id={otherEntityUri}";

                var requestUri = requestUriBuilder.Uri;
                var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
                return httpRequest;
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEntity<TResult>> InvokeAsync<TResult>(
            ODataFunctionRequest<TResult> request,
            CancellationToken cancellationToken = default
        )
            where TResult : IEntity
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var arguments = string.Join(",", request.Arguments.Select(kvp => $"{kvp.Key}={_expressionFormatter.ToString(kvp.Value)}"));
                _logger.LogDebug("Invoking function '{Function}' with arguments ({Arguments})...", request.FunctionName, arguments);
            }
            
            Task<HttpRequestMessage> CreateRequest()
            {
                var requestUri = FunctionUri(request);
                var httpRequest = CreateDefaultRequest(HttpMethod.Get, requestUri);
                return Task.FromResult(httpRequest);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);

            var entity = await httpResponse.Content.ReadEntityAsync(request.EntityType, _entitySerializer, cancellationToken);
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var arguments = string.Join(",", request.Arguments.Select(kvp => $"{kvp.Key}={_expressionFormatter.ToString(kvp.Value)}"));
                _logger.LogDebug("Invocation of function '{Function}' with arguments ({Arguments}) returned: {Result}", request.FunctionName, arguments, entity.ToJson(Formatting.None));
            }
            
            return entity;
        }

        private async Task<HttpRequestMessage> CreatePropsRequestAsync<TEntity>(
            IEntityType<TEntity> type,
            HttpMethod method,
            Uri requestUri,
            Action<IODataProperties<TEntity>> props
        )
            where TEntity : IEntity
        {
            var context = ODataPropertiesFactoryContext.Create(this, _entitySetNameResolver, type);
            var properties = _propertiesFactory.Create(context);
            props(properties);

            var httpContent = await properties.ToHttpContentAsync();
            return CreateMutationRequest(method, requestUri, httpContent);
        }

        private async Task<HttpRequestMessage> CreatePropsRepresentationRequestAsync<TEntity>(
            IEntityType<TEntity> type,
            HttpMethod method,
            Uri requestUri,
            Action<IODataProperties<TEntity>> props
        )
            where TEntity : IEntity
        {
            var httpRequest = await CreatePropsRequestAsync(type, method, requestUri, props);
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