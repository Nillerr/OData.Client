using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataAuthenticator
    {
        Task AddAuthenticationAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
    }
}