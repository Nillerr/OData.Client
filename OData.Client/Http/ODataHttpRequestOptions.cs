using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace OData.Client
{
    /// <summary>
    /// Options to apply to to the HTTP request.
    /// </summary>
    public sealed class ODataHttpRequestOptions
    {
        /// <summary>
        /// Status codes that shouldn't trigger an automatic <see cref="HttpRequestException"/>, e.g. when
        /// <c>404 Not Found</c> is considered a valid result and must be translated to a <see langword="null"/> return
        /// value.
        /// </summary>
        public HashSet<HttpStatusCode> AllowedStatusCodes { get; } = new();
    }
}