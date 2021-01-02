using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OData.Client.Newtonsoft.Json
{
    internal sealed class JObjectFindResponse<TEntity> : IFindResponse<TEntity>
        where TEntity : IEntity
    {
        public JObjectFindResponse(Uri context, Uri? nextLink, List<JObjectEntity<TEntity>> value)
        {
            Context = context;
            NextLink = nextLink;
            Value = value.AsReadOnly();
        }

        [JsonProperty("@odata.context")]
        public Uri Context { get; }
        
        [JsonProperty("@odata.nextLink", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? NextLink { get; }
        
        [JsonProperty("value")]
        public IReadOnlyList<IEntity<TEntity>> Value { get; }
    }
}