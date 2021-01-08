using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// An authenticator for authenticating requests with the OData API.
    /// </summary>
    public interface IODataAuthenticator
    {
        /// <summary>
        /// Authorize a request to the OData API.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task AuthorizeAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
    }
}