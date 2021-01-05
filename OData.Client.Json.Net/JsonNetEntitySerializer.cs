using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    /// <inheritdoc />
    public sealed class JsonNetEntitySerializer : IEntitySerializer
    {
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetEntitySerializer"/> class.
        /// </summary>
        public JsonNetEntitySerializer()
        {
            _serializer = JsonSerializer.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetEntitySerializer"/> class.
        /// </summary>
        /// <param name="settings">The settings to be applied to the resulting <see cref="JsonSerializer"/>.</param>
        public JsonNetEntitySerializer(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetEntitySerializer"/> class.
        /// </summary>
        /// <param name="serializer">The serializer to use.</param>
        public JsonNetEntitySerializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc />
        public ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(
            Stream stream,
            IEntityType<TEntity> entityType
        )
            where TEntity : IEntity
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
            var values = root.GetValue<JArray>("value", _serializer);
            var value = ToEntities(values, entityType);
            
            var response = new FindResponse<TEntity>(entityType, context, nextLink, value);
            return ValueTask.FromResult<IFindResponse<TEntity>>(response);
        }
        
        private List<JObjectEntity<TEntity>> ToEntities<TEntity>(JArray value, IEntityType<TEntity> entityType)
            where TEntity : IEntity
        {
            var entities = new List<JObjectEntity<TEntity>>(value.Count);
            
            foreach (var token in value)
            {
                if (token is not JObject root)
                {
                    throw new JsonSerializationException($"Unexpected token '{token.Type:G}', expected '{JTokenType.Object}'.");
                }

                var entity = new JObjectEntity<TEntity>(entityType, root, _serializer);
                entities.Add(entity);
            }

            return entities;
        }

        /// <inheritdoc />
        public ValueTask<IEntity<TEntity>> DeserializeEntityAsync<TEntity>(
            Stream stream,
            IEntityType<TEntity> entityType
        )
            where TEntity : IEntity
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var root = _serializer.Deserialize<JObject>(jsonReader);
            if (root == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }

            var entity = new JObjectEntity<TEntity>(entityType, root, _serializer);
            return ValueTask.FromResult<IEntity<TEntity>>(entity);
        }
    }
}