using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Newtonsoft.Json
{
    internal sealed class JObjectFindResponse<TEntity> : IFindResponse<TEntity>
        where TEntity : IEntity
    {
        public JObjectFindResponse(Uri context, Uri? nextLink, List<JObject> value)
        {
            Context = context;
            NextLink = nextLink;

            var entities = ToEntities(value, context);
            Value = entities.AsReadOnly();
        }

        [JsonProperty("@odata.context")]
        public Uri Context { get; }

        [JsonProperty("@odata.nextLink", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? NextLink { get; }

        [JsonProperty("value")]
        public IReadOnlyList<IEntity<TEntity>> Value { get; }

        private static List<JObjectEntity<TEntity>> ToEntities(List<JObject> value, Uri context)
        {
            var entityName = EntityNameFromContext(context);
            
            var entities = new List<JObjectEntity<TEntity>>(value.Count);
            foreach (var root in value)
            {
                var entity = new JObjectEntity<TEntity>(root, entityName);
                entities.Add(entity);
            }

            return entities;
        }

        private static EntityName<TEntity> EntityNameFromContext(Uri context)
        {
            var contextString = context.ToString();
            var startIndex = contextString.IndexOf("/$metadata#", StringComparison.Ordinal) + 11;
            var entityNameString = contextString.Substring(startIndex, contextString.IndexOf('(', startIndex) - startIndex);
            var entityName = new EntityName<TEntity>(entityNameString);
            return entityName;
        }
    }
}