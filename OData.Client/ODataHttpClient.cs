using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public sealed class ODataHttpClient : IODataHttpClient
    {
        private static readonly HttpRequestOptions DefaultRequestOptions = new HttpRequestOptions();
        
        private readonly IClock _clock;
        private readonly IODataAuthenticator _authenticator;
        private readonly IHttpClientProvider _httpClientProvider;

        public ODataHttpClient(IClock clock, IODataAuthenticator authenticator, IHttpClientProvider httpClientProvider)
        {
            _clock = new UtcClock(clock);
            _authenticator = authenticator;
            _httpClientProvider = httpClientProvider;
            
            RetryAt = _clock.UtcNow;
        }

        public DateTime RetryAt { get; private set; }

        public async Task<HttpResponseMessage> SendAsync(
            Func<HttpRequestMessage> requestFactory,
            HttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        )
        {
            var requestOptions = options ?? DefaultRequestOptions;
            
            const int maximumNumberOfAttempts = 5;
            var attempt = 0;

            var shouldRetryRequest = true;
            while (shouldRetryRequest)
            {
                var delay = RetryAt - _clock.UtcNow;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, cancellationToken);
                }

                using var request = requestFactory();
                await _authenticator.AddAuthenticationAsync(request, cancellationToken);

                var httpClient = _httpClientProvider.HttpClient;
                var response = await httpClient.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    RetryAt = response.Headers.GetRetryAt(_clock.UtcNow);

                    shouldRetryRequest = attempt < maximumNumberOfAttempts;
                    attempt++;

                    continue;
                }

                if (requestOptions.AllowedStatusCodes.Contains(response.StatusCode))
                {
                    return response;
                }

                var stringContent = await response.Content.ReadAsStringAsync(cancellationToken);

                // Not 429 Too Many Requests status code
                var message = $"The request returned an unsuccessful status code {response.StatusCode:D} {response.ReasonPhrase ?? (response.StatusCode.ToString("G"))}: {stringContent}";
                throw new HttpRequestException(message, null, response.StatusCode);
            }

            // Ran out of attempts
            throw new HttpRequestException();
        }
    }
}