using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// A wrapper around <see cref="HttpClient"/> which handles rate limiting and attaches authorization headers to
    /// the request using the supplied <see cref="IODataAuthenticator"/>. If any request made using this class
    /// encounters a <c>429 Too Many Requests</c> response, the request will be retried at the time specified by the
    /// accompanying <c>Retry-After</c> header, and subsequent requests will wait until the receiver is ready to
    /// receive requests before submitting theirs.
    /// </summary>
    public sealed class RateLimitedODataHttpClient : IODataHttpClient
    {
        private readonly IClock _clock;
        private readonly IODataHttpClient _oDataHttpClient;
        private readonly IRateLimitPolicy _rateLimitPolicy;

        public RateLimitedODataHttpClient(IClock clock, IODataHttpClient oDataHttpClient, IRateLimitPolicy rateLimitPolicy)
        {
            _clock = new UtcClock(clock);
            _oDataHttpClient = oDataHttpClient;
            _rateLimitPolicy = rateLimitPolicy;

            RetryAt = _clock.UtcNow;
        }

        public DateTime RetryAt { get; private set; }

        public async Task<HttpResponseMessage> SendAsync(
            Func<ValueTask<HttpRequestMessage>> requestFactory,
            ODataHttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        )
        {
            var attempts = 0;

            var shouldRetryRequest = true;
            while (shouldRetryRequest)
            {
                var delay = RetryAt - _clock.UtcNow;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, cancellationToken);
                }

                var httpResponse = await _oDataHttpClient.SendAsync(requestFactory, options, cancellationToken);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return httpResponse;
                }

                if (httpResponse.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var retryAt = httpResponse.Headers.GetRetryAt(_clock.UtcNow);
                    RetryAt = retryAt;

                    attempts++;
                    
                    var context = new RateLimitPolicyContext(httpResponse, retryAt, attempts);
                    shouldRetryRequest = _rateLimitPolicy.ShouldRetry(context);

                    continue;
                }

                var stringContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

                // Not 429 Too Many Requests status code
                var message = $"The request returned an unsuccessful status code {httpResponse.StatusCode:D} {httpResponse.ReasonPhrase ?? (httpResponse.StatusCode.ToString("G"))}: {stringContent}";
                throw new HttpRequestException(message, null, httpResponse.StatusCode);
            }

            // Ran out of attempts
            throw new HttpRequestException();
        }
    }
}