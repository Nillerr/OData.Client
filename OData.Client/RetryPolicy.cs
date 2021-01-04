using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public sealed class RetryPolicy : IRetryPolicy
    {
        public async Task<bool?> ShouldRetryAsync(
            RateLimitPolicyContext context,
            CancellationToken cancellationToken = default
        )
        {
            var response = context.Response;
            if (response.StatusCode != HttpStatusCode.TooManyRequests)
            {
                throw new HttpRequestException();
            }

            var retryAfterHeader = response.Headers.RetryAfter;
            if (retryAfterHeader == null)
            {
                throw new HttpRequestException();
            }

            var delta = retryAfterHeader.Delta;
            if (delta == null)
            {
                throw new HttpRequestException();
            }

            await Task.Delay(delta.Value, cancellationToken);

            throw new HttpRequestException();
        }
    }
}