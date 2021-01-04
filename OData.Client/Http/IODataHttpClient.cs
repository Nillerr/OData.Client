using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// A wrapper around <see cref="HttpClient"/>.
    /// </summary>
    public interface IODataHttpClient
    {
        Task<HttpResponseMessage> SendAsync(
            Func<HttpRequestMessage> requestFactory,
            ODataHttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        );
    }
}