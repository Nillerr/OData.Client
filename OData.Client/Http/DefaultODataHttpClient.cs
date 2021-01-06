using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// A wrapper around <see cref="HttpClient"/>.
    /// </summary>
    public sealed class DefaultODataHttpClient : IODataHttpClient
    {
        private static readonly ODataHttpRequestOptions DefaultRequestOptions = new ODataHttpRequestOptions();
        
        private readonly IHttpClientProvider _httpClientProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultODataHttpClient"/> class.
        /// </summary>
        public DefaultODataHttpClient(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public async Task<HttpResponseMessage> SendAsync(
            Func<Task<HttpRequestMessage>> requestFactory,
            ODataHttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        )
        {
            var requestOptions = options ?? DefaultRequestOptions;
            
            var httpRequest = await requestFactory();
            
            var httpClient = _httpClientProvider.HttpClient;
            
            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            if (httpResponse.IsSuccessStatusCode)
            {
                return httpResponse;
            }

            if (requestOptions.AllowedStatusCodes.Contains(httpResponse.StatusCode))
            {
                return httpResponse;
            }
            
            var stringContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            // Not 429 Too Many Requests status code
            var statusCode = httpResponse.StatusCode.ToString("D");
            var reason = httpResponse.ReasonPhrase ?? (httpResponse.StatusCode.ToString("G"));
            
            var message = $"Response status does not indicate success: {statusCode} {reason}.{Environment.NewLine}" +
                          $"{stringContent}";
            
            throw new HttpRequestException(message, null, httpResponse.StatusCode);
        }
    }
}