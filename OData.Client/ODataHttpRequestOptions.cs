using System.Collections.Generic;
using System.Net;

namespace OData.Client
{
    public sealed class ODataHttpRequestOptions
    {
        public HashSet<HttpStatusCode> AllowedStatusCodes { get; } = new();
    }
}