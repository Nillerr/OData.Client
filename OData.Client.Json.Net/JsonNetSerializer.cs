using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetSerializer<TEntity> : ISerializer<TEntity> where TEntity : IEntity
    {
        private readonly IEntityName<TEntity> _entityName;
        private readonly JsonSerializer _serializer;
        private readonly JsonSerializerFactory _serializerFactory;

        public JsonNetSerializer(IEntityName<TEntity> entityName, JsonSerializerFactory serializerFactory)
        {
            _entityName = entityName;
            _serializer = serializerFactory.CreateSerializer(entityName);
            _serializerFactory = serializerFactory;
        }

        public ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync(
            Stream stream,
            ODataFindRequest<TEntity> request
        )
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var root = _serializer.Deserialize<JObject>(jsonReader);
            if (root == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }

            var context = root.GetValue<Uri>("@odata.context", _serializer);
            var nextLink = root.GetValueOrDefault<Uri>("@odata.nextLink", _serializer);
            var value = root.GetValue<JArray>("value", _serializer).ToEntities(_entityName, _serializerFactory);
            var response = new FindResponse<TEntity>(_entityName, context, nextLink, value, request);
            
            return ValueTask.FromResult<IFindResponse<TEntity>>(response);
        }

        public ValueTask<IEntity<TEntity>> DeserializeEntityAsync(Stream stream)
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var root = _serializer.Deserialize<JObject>(jsonReader);
            if (root == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }

            var entity = root.ToEntity(_entityName, _serializerFactory);
            return ValueTask.FromResult<IEntity<TEntity>>(entity);
        }
    }
}