using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetEntitySerializer : IEntitySerializer
    {
        private readonly JsonSerializer _serializer;

        public JsonNetEntitySerializer()
        {
            _serializer = JsonSerializer.Create();
        }

        public JsonNetEntitySerializer(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }

        public JsonNetEntitySerializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(
            Stream stream,
            IODataFindRequest<TEntity> request
        ) where TEntity : IEntity
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var token = _serializer.Deserialize<JToken>(jsonReader);
            if (token is not JObject root)
            {
                var tokenType = token?.Type ?? JTokenType.Null;
                throw new JsonSerializationException($"Unexpected token '{tokenType:G}', expected '{JTokenType.Object}'.");
            }

            var context = root.GetValue<Uri>("@odata.context", _serializer);
            var nextLink = root.GetValueOrDefault<Uri>("@odata.nextLink", _serializer);
            var value = root.GetValue<JArray>("value", _serializer).ToEntities<TEntity>(_serializer);
            var response = new FindResponse<TEntity>(context, nextLink, value, request);
            
            return ValueTask.FromResult<IFindResponse<TEntity>>(response);
        }

        public ValueTask<IEntity<TEntity>> DeserializeEntityAsync<TEntity>(Stream stream) where TEntity : IEntity
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var root = _serializer.Deserialize<JObject>(jsonReader);
            if (root == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }

            var entity = ToEntity<TEntity>(root);
            return ValueTask.FromResult<IEntity<TEntity>>(entity);
        }

        private JObjectEntity<TEntity> ToEntity<TEntity>(JObject value) where TEntity : IEntity
        {
            var entity = new JObjectEntity<TEntity>(value, _serializer);
            return entity;
        }
    }
}