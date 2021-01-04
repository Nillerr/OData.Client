using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataHttpClient
    {
        Task<HttpResponseMessage> SendAsync(
            Func<HttpRequestMessage> requestFactory,
            ODataHttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        );
    }
}