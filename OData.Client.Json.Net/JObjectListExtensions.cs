using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal static class JObjectListExtensions
    {
        public static List<JObjectEntity<TEntity>> ToEntities<TEntity>(this JArray value, Uri context) where TEntity : IEntity
        {
            var entityName = EntityNameFromContext<TEntity>(context);
            
            var entities = new List<JObjectEntity<TEntity>>(value.Count);
            foreach (var token in value)
            {
                if (token is not JObject root)
                {
                    // TODO Throw a JSON Serialization Exception
                    throw new JsonSerializationException("Throw a JSON Serialization Exception");
                }
                
                var entity = new JObjectEntity<TEntity>(root, entityName);
                entities.Add(entity);
            }

            return entities;
        }

        public static JObjectEntity<TEntity> ToEntity<TEntity>(this JObject value) where TEntity : IEntity
        {
            var context = value.Value<Uri>("@odata.context");
            var entityName = EntityNameFromContext<TEntity>(context);
            var entity = new JObjectEntity<TEntity>(value, entityName);
            return entity;
        }

        public static EntityName<TEntity> EntityNameFromContext<TEntity>(Uri context) where TEntity : IEntity
        {
            var contextString = context.ToString();
            var startIndex = contextString.IndexOf("/$metadata#", StringComparison.Ordinal) + 11;
            var entityNameString = contextString.Substring(startIndex, contextString.IndexOf('(', startIndex) - startIndex);
            var entityName = new EntityName<TEntity>(entityNameString);
            return entityName;
        }
    }
}