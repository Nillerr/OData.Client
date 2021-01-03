using System;
using System.Collections.Generic;
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
        private readonly HttpClient _httpClient;
        private readonly IODataPropertiesFactory _propertiesFactory;
        private readonly ISerializerFactory _serializerFactory;
        private readonly IValueFormatter _valueFormatter;

        public ODataClient(
            Uri organizationUri,
            HttpClient httpClient,
            IODataPropertiesFactory propertiesFactory,
            ISerializerFactory serializerFactory,
            IValueFormatter valueFormatter
        )
        {
            _organizationUri = organizationUri;
            _httpClient = httpClient;
            _propertiesFactory = propertiesFactory;
            _serializerFactory = serializerFactory;
            _valueFormatter = valueFormatter;
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

        private Uri PropertyRefUri<TEntity>(IEntityId<TEntity> id, IProperty<TEntity> property)
            where TEntity : IEntity
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
            var requestUri = FindUri(name, request);
            
            using var httpRequest = CreateFindRequest(HttpMethod.Get, requestUri, request);
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var serializer = _serializerFactory.CreateSerializer(name);
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
            
            using var httpRequest = CreateFindRequest(HttpMethod.Get, requestUri, request);
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
            
            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            var serializer = _serializerFactory.CreateSerializer(current.EntityName);
            var response = await serializer.DeserializeFindResponseAsync(stream, request);
            return response;
        }

        private static HttpRequestMessage CreateFindRequest<TEntity>(
            HttpMethod method,
            Uri requestUri,
            ODataFindRequest<TEntity> request
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
            var requestUri = RetrieveUri(id, request);

            using var httpRequest = CreateDefaultRequest(HttpMethod.Get, requestUri);
            using var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ThrowIfUnsuccessfulAsync(httpResponse, cancellationToken);

            var serializer = _serializerFactory.CreateSerializer(id.Name);
            var entity = await httpResponse.Content.ReadEntityAsync(serializer, cancellationToken);
            return entity;
        }

        public async Task<IEntityId<TEntity>> CreateAsync<TEntity>(
            IEntityName<TEntity> name,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var requestUri = CollectionUri(name);

            using var httpRequest = CreatePropsRequest(HttpMethod.Post, requestUri, props);
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);

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
            var requestUri = CollectionUri(name);

            using var httpRequest = CreatePropsRequest(HttpMethod.Post, requestUri, props);
            httpRequest.Headers.Add("Prefer", "return=representation");
            
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
            
            var serializer = _serializerFactory.CreateSerializer(name);
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
            var requestUri = EntityUri(id);

            using var httpRequest = CreatePropsRequest(HttpMethod.Patch, requestUri, props);
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
        }

        public async Task<IEntity<TEntity>> UpdateRepresentationAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var requestUri = EntityUri(id);

            using var httpRequest = CreatePropsRequest(HttpMethod.Patch, requestUri, props);
            httpRequest.Headers.Add("Prefer", "return=representation");
            
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
            
            var serializer = _serializerFactory.CreateSerializer(id.Name);
            var entity = await httpResponse.Content.ReadEntityAsync(serializer, cancellationToken);
            return entity;
        }

        public async Task DeleteAsync<TEntity>(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
            where TEntity : IEntity
        {
            var requestUri = EntityUri(id);

            using var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
            httpRequest.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
        }

        public async Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, TOther?> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var requestUri = PropertyRefUri(id, property);

            using var httpRequest = CreatePropsRequest<Ref>(HttpMethod.Put, requestUri, props =>
            {
                props.Set(Ref.Id, EntityUri(otherId));
            });

            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
        }

        public async Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var requestUri = PropertyRefUri(id, property);

            using var httpRequest = CreatePropsRequest<Ref>(HttpMethod.Put, requestUri, props =>
            {
                props.Set(Ref.Id, EntityUri(otherId));
            });

            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
        }

        public async Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, TOther?> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var requestUri = PropertyRefUri(id, property);

            using var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
        }

        public async Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var propertyRefUri = PropertyRefUri(id, property);
            var otherEntityUri = EntityUri(otherId);

            var requestUriBuilder = new UriBuilder(propertyRefUri);
            requestUriBuilder.Query = $"$id={otherEntityUri}";

            var requestUri = requestUriBuilder.Uri;

            using var httpRequest = CreateDefaultRequest(HttpMethod.Delete, requestUri);
            using var httpResponse = await SendRequestAsync(httpRequest, cancellationToken);
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

        private async Task<HttpResponseMessage> SendRequestAsync(
            HttpRequestMessage httpRequest,
            CancellationToken cancellationToken
        )
        {
            var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            await ThrowIfUnsuccessfulAsync(httpResponse, cancellationToken);
            return httpResponse;
        }

        private static async Task ThrowIfUnsuccessfulAsync(
            HttpResponseMessage httpResponse,
            CancellationToken cancellationToken
        )
        {
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