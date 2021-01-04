using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// The context used in <see cref="IRateLimitPolicy.ShouldRetry"/>.
    /// </summary>
    [PublicAPI]
    public sealed class RateLimitPolicyContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RateLimitPolicyContext"/> class.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="retryAt">The time to retry at, in UTC.</param>
        /// <param name="attempts">The number of attempts so far.</param>
        public RateLimitPolicyContext(HttpResponseMessage response, DateTime retryAt, int attempts)
        {
            Response = response;
            RetryAt = retryAt;
            Attempts = attempts;
        }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        public HttpResponseMessage Response { get; }
        
        /// <summary>
        /// The time to retry at, in UTC.
        /// </summary>
        public DateTime RetryAt { get; }
        
        /// <summary>
        /// The number of attempts so far.
        /// </summary>
        public int Attempts { get; }
    }
}