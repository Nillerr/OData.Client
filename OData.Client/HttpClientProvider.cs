using System;
using System.Net.Http;

namespace OData.Client
{
    public sealed class HttpClientProvider : IHttpClientProvider, IDisposable
    {
        private readonly bool _disposeHttpClient;

        public HttpClientProvider()
        {
            HttpClient = new HttpClient();
            _disposeHttpClient = true;
        }
        
        public HttpClientProvider(HttpClient httpClient)
        {
            HttpClient = httpClient;
            _disposeHttpClient = true;
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                HttpClient.Dispose();
            }
        }
    }
}