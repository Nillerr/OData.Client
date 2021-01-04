using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal static class JObjectListExtensions
    {
        public static List<JObjectEntity<TEntity>> ToEntities<TEntity>(this JArray value, JsonSerializer serializer)
            where TEntity : IEntity
        {
            var entities = new List<JObjectEntity<TEntity>>(value.Count);
            foreach (var token in value)
            {
                if (token is not JObject root)
                {
                    throw new JsonSerializationException($"Unexpected token '{token.Type:G}', expected '{JTokenType.Object}'.");
                }

                var entity = new JObjectEntity<TEntity>(root, serializer);
                entities.Add(entity);
            }

            return entities;
        }
    }
}