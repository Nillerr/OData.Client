using System;
using System.Net.Http;

namespace OData.Client
{
    public sealed class DefaultHttpClientProvider : IHttpClientProvider, IDisposable
    {
        private readonly bool _disposeHttpClient;
        
        public DefaultHttpClientProvider()
        {
            HttpClient = new HttpClient();
            _disposeHttpClient = true;
        }
        
        public DefaultHttpClientProvider(HttpClient httpClient)
        {
            HttpClient = httpClient;
            _disposeHttpClient = false;
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