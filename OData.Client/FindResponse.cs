using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OData.Client
{
    public sealed record FindResponse<TEntity>(
        [property: JsonPropertyName("@odata.context")]
        string Context,
        [property: JsonPropertyName("value")] List<TEntity> Value,
        [property: JsonPropertyName("@odata.nextLink")]
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        Uri? NextLink
    ) : IFindResponse<TEntity>;
}