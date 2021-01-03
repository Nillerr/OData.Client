using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal static class JObjectListExtensions
    {
        public static List<JObjectEntity<TEntity>> ToEntities<TEntity>(
            this JArray value,
            IEntityName<TEntity> name,
            JsonSerializerFactory serializerFactory
        ) where TEntity : IEntity
        {
            var serializer = serializerFactory.CreateSerializer(name);
            
            var entities = new List<JObjectEntity<TEntity>>(value.Count);
            foreach (var token in value)
            {
                if (token is not JObject root)
                {
                    // TODO Throw a JSON Serialization Exception
                    throw new JsonSerializationException("Throw a JSON Serialization Exception");
                }

                var entity = new JObjectEntity<TEntity>(root, serializer, serializerFactory);
                entities.Add(entity);
            }

            return entities;
        }

        public static JObjectEntity<TEntity> ToEntity<TEntity>(
            this JObject value,
            IEntityName<TEntity> name,
            JsonSerializerFactory serializerFactory
        ) where TEntity : IEntity
        {
            var serializer = serializerFactory.CreateSerializer(name);
            var entity = new JObjectEntity<TEntity>(value, serializer, serializerFactory);
            return entity;
        }
    }
}