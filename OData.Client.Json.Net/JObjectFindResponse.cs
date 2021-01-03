using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal sealed class JObjectFindResponse<TEntity> : IFindResponse<TEntity>
        where TEntity : IEntity
    {
        public JObjectFindResponse(Uri context, Uri? nextLink, List<JObject> value)
        {
            Context = context;
            NextLink = nextLink;

            var entities = value.ToEntities<TEntity>(context);
            Value = entities.AsReadOnly();
        }

        [JsonProperty("@odata.context")]
        public Uri Context { get; }

        [JsonProperty("@odata.nextLink", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? NextLink { get; }

        [JsonProperty("value")]
        public IReadOnlyList<IEntity<TEntity>> Value { get; }

        public ODataFindRequest<TEntity> Request { get; set; }
    }
}