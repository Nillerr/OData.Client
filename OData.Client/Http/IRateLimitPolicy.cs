namespace OData.Client
{
    /// <summary>
    /// Determines whether a caller should retry or not when requesting a resource.
    /// </summary>
    /// <seealso cref="RateLimitedODataHttpClient"/>
    public interface IRateLimitPolicy
    {
        /// <summary>
        /// Determines whether a caller should retry or not when requesting a resource.
        /// </summary>
        /// <param name="context">The rate limit policy context.</param>
        /// <returns><see langword="true"/> if the caller should retry; otherwise <see langword="false"/>.</returns>
        bool ShouldRetry(RateLimitPolicyContext context);
    }
}