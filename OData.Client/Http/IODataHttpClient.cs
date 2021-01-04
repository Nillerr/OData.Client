using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A wrapper around <see cref="HttpClient"/>.
    /// </summary>
    public interface IODataHttpClient
    {
        /// <summary>
        /// Sends the request created by <paramref name="requestFactory"/>.
        /// </summary>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="options">The request options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The HTTP response.</returns>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network
        /// connectivity, DNS failure, server certificate validation or timeout, or the HTTP response message if the
        /// call is successful.</exception>
        Task<HttpResponseMessage> SendAsync(
            [InstantHandle] Func<ValueTask<HttpRequestMessage>> requestFactory,
            ODataHttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        );
    }
}