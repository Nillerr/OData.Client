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
    public sealed class ODataClient : IODataClient
    {
        private readonly Uri _organizationUri;
        private readonly IODataPropertiesFactory _propertiesFactory;
        private readonly IEntitySerializerFactory _entitySerializerFactory;
        private readonly IODataHttpClient _oDataHttpClient;
        private readonly IValueFormatter _valueFormatter;

        public ODataClient(ODataClientSettings settings)
        {
            _organizationUri = settings.OrganizationUri;
            _propertiesFactory = settings.PropertiesFactory;
            _entitySerializerFactory = settings.EntitySerializerFactory;
            _oDataHttpClient = settings.HttpClient;
            _valueFormatter = settings.ValueFormatter;
        }

        private Uri BaseUri => new Uri(_organizationUri, "api/data/v9.1/");

        private Uri CollectionUri<T>(IEntityName<T> name) where T : IEntity => new Uri(BaseUri, $"{name.Name}");

        private Uri EntityUri<T>(IEntityId<T> id) where T : IEntity => new Uri(BaseUri, $"{id.Name.Name}({id.Id:D})");

        private Uri FindUri<TEntity>(IEntityName<TEntity> name, ODataFindRequest<TEntity> request)
            where TEntity : IEntity
        {
            var collectionUri = CollectionUri(name);

            var requestUriBuilder = new UriBuilder(collectionUri);
            requestUriBuilder.Query = request.ToQueryString(_valueFormatter, QueryStringFormatting.Escaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private Uri RetrieveUri<TEntity>(IEntityId<TEntity> id, ODataRetrieveRequest<TEntity> request)
            where TEntity : IEntity
        {
            var entityUri = EntityUri(id);

            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Query = request.ToQueryString(QueryStringFormatting.Escaped);

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        private Uri PropertyRefUri<TEntity, TOther>(IEntityId<TEntity> id, IRefProperty<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            var entityUri = EntityUri(id);

            var requestUriBuilder = new UriBuilder(entityUri);
            requestUriBuilder.Path += $"/{property.Name}/$ref";

            var requestUri = requestUriBuilder.Uri;
            return requestUri;
        }

        public IODataCollection<TEntity> Collection<TEntity>(IEntityName<TEntity> name) where TEntity : IEntity
        {
            return new ODataCollection<TEntity>(name, this, _valueFormatter);
        }

        public async Task<IFindResponse<TEntity>> FindAsync<TEntity>(
            IEntityName<TEntity> name,
            ODataFindRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = FindUri(name, request);
                return CreateFindRequest(HttpMethod.Get, requestUri, request);
            }
            
            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var serializer = _entitySerializerFactory.CreateSerializer(name);
            var response = await serializer.DeserializeFindResponseAsync(stream, request);
            return response;
        }

        public async Task<IFindResponse<TEntity>?> FindNextAsync<TEntity>(
            IFindResponse<TEntity> current,
            CancellationToken cancellationToken = default
        ) where TEntity : IEntity
        {
            var requestUri = current.NextLink;
            if (requestUri == null)
            {
                return null;
            }
            
            var request = current.Request;
            
            HttpRequestMessage CreateRequest()
            {
                return CreateFindRequest(HttpMethod.Get, requestUri, request);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var serializer = _entitySerializerFactory.CreateSerializer(current.EntityName);
            var response = await serializer.DeserializeFindResponseAsync(stream, request);
            return response;
        }

        private static HttpRequestMessage CreateFindRequest<TEntity>(
            HttpMethod method,
            Uri requestUri,
            IODataFindRequest<TEntity> request
        )
            where TEntity : IEntity
        {
            var httpRequest = CreateDefaultRequest(method, requestUri);
            
            if (request.MaxPageSize.HasValue)
            {
                httpRequest.Headers.Add("Prefer", $"odata.maxpagesize={request.MaxPageSize.Value}");
            }
            
            return httpRequest;
        }

        public async Task<IEntity<TEntity>?> RetrieveAsync<TEntity>(
            IEntityId<TEntity> id,
            ODataRetrieveRequest<TEntity> request,
            CancellationToken cancellationToken = default
        ) 
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = RetrieveUri(id, request);
                return CreateDefaultRequest(HttpMethod.Get, requestUri);
            }

            var httpRequestOptions = new ODataHttpRequestOptions();
            httpRequestOptions.AllowedStatusCodes.Add(HttpStatusCode.NotFound);
            
            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, httpRequestOptions, cancellationToken);
            
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var serializer = _entitySerializerFactory.CreateSerializer(id.Name);
            var entity = await httpResponse.Content.ReadEntityAsync(serializer, cancellationToken);
            return entity;
        }

        public async Task<EntityId<TEntity>> CreateAsync<TEntity>(
            IEntityName<TEntity> name,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = CollectionUri(name);
                return CreatePropsRequest(HttpMethod.Post, requestUri, props);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);

            var entityId = httpResponse.Headers.EntityId(name);
            return entityId;
        }

        public async Task<IEntity<TEntity>> CreateRepresentationAsync<TEntity>(
            IEntityName<TEntity> name,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = CollectionUri(name);
                return CreatePropsRepresentationRequest(HttpMethod.Post, requestUri, props);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            var serializer = _entitySerializerFactory.CreateSerializer(name);
            var entity = await httpResponse.Content.ReadEntityAsync(serializer, cancellationToken);
            return entity;
        }

        public async Task UpdateAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = EntityUri(id);
                return CreatePropsRequest(HttpMethod.Patch, requestUri, props);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        public async Task<IEntity<TEntity>> UpdateRepresentationAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = EntityUri(id);
                return CreatePropsRepresentationRequest(HttpMethod.Patch, requestUri, props);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
            
            var serializer = _entitySerializerFactory.CreateSerializer(id.Name);
            var entity = await httpResponse.Content.ReadEntityAsync(serializer, cancellationToken);
            return entity;
        }

        public async Task DeleteAsync<TEntity>(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
            where TEntity : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                return CreateEmptyRequest(HttpMethod.Delete, EntityUri(id));
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        public async Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                return CreatePropsRequest<Ref>(HttpMethod.Put, PropertyRefUri(id, property), props =>
                {
                    props.Set(Ref.Id, EntityUri(otherId));
                });
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        public async Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                return CreatePropsRequest<Ref>(HttpMethod.Put, PropertyRefUri(id, property), props =>
                {
                    props.Set(Ref.Id, EntityUri(otherId));
                });
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        public async Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var requestUri = PropertyRefUri(id, property);
                return CreateDefaultRequest(HttpMethod.Delete, requestUri);
            }

            using var httpResponse = await _oDataHttpClient
                .SendAsync(CreateRequest, cancellationToken: cancellationToken);
        }

        public async Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            HttpRequestMessage CreateRequest()
            {
                var propertyRefUri = PropertyRefUri(id, property);
                var otherEntityUri = EntityUri(otherId);

                var requestUriBuilder = new UriBuilder(propertyRefUri);
                requestUriBuilder.Query = $"$id={otherEntityUri}";

                var requestUri = requestUriBuilder.Uri;
                return CreateDefaultRequest(HttpMethod.Delete, requestUri);
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