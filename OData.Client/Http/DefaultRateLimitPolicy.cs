namespace OData.Client
{
    /// <summary>
    /// Determines whether a caller should retry a request based on the number of attempts.
    /// </summary>
    public sealed class DefaultRateLimitPolicy : IRateLimitPolicy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRateLimitPolicy"/> class.
        /// </summary>
        /// <param name="maximumNumberOfAttempts">The maximum number of attempts.</param>
        public DefaultRateLimitPolicy(int maximumNumberOfAttempts = 5)
        {
            MaximumNumberOfAttempts = maximumNumberOfAttempts;
        }

        /// <summary>
        /// The maximum number of attempts.
        /// </summary>
        public int MaximumNumberOfAttempts { get; }
        
        /// <summary>
        /// Determines whether a caller has reached the maximum number of attempts.
        /// </summary>
        /// <param name="context">The rate limit policy context.</param>
        /// <returns><see langword="true"/> if the maximum number of attempts has been reached; otherwise <see langword="false"/>.</returns>
        public bool ShouldRetry(RateLimitPolicyContext context)
        {
            return context.Attempts < MaximumNumberOfAttempts;
        }
    }
}