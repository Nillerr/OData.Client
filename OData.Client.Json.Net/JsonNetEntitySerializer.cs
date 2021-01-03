using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetEntitySerializer<TEntity> : IEntitySerializer<TEntity> where TEntity : IEntity
    {
        private readonly IEntityName<TEntity> _entityName;
        private readonly JsonSerializer _serializer;
        private readonly IJsonSerializerFactory _serializerFactory;

        public JsonNetEntitySerializer(IEntityName<TEntity> entityName, IJsonSerializerFactory serializerFactory)
        {
            _entityName = entityName;
            _serializer = serializerFactory.CreateSerializer(entityName);
            _serializerFactory = serializerFactory;
        }

        public ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync(
            Stream stream,
            IODataFindRequest<TEntity> request
        )
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
            var value = root.GetValue<JArray>("value", _serializer).ToEntities(_entityName, _serializerFactory);
            var response = new FindResponse<TEntity>(_entityName, context, nextLink, value, request);
            
            return ValueTask.FromResult<IFindResponse<TEntity>>(response);
        }
        
        private List<JObjectEntity<TEntity>> ToEntities(JArray value)
        {
            var serializer = _serializerFactory.CreateSerializer(_entityName);
            
            var entities = new List<JObjectEntity<TEntity>>(value.Count);
            foreach (var token in value)
            {
                if (token is not JObject root)
                {
                    throw new JsonSerializationException($"Unexpected token '{token.Type:G}', expected '{JTokenType.Object}'.");
                }

                var entity = new JObjectEntity<TEntity>(root, serializer, _serializerFactory);
                entities.Add(entity);
            }

            return entities;
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

            var entity = ToEntity(root);
            return ValueTask.FromResult<IEntity<TEntity>>(entity);
        }

        private JObjectEntity<TEntity> ToEntity(JObject value)
        {
            var serializer = _serializerFactory.CreateSerializer(_entityName);
            var entity = new JObjectEntity<TEntity>(value, serializer, _serializerFactory);
            return entity;
        }
    }
}