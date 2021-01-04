using System.Net.Http;

namespace OData.Client
{
    public interface IHttpClientProvider
    {
        HttpClient HttpClient { get; }
    }
}