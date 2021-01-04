namespace OData.Client
{
    public sealed class DefaultRateLimitPolicy : IRateLimitPolicy
    {
        public DefaultRateLimitPolicy(int maximumNumberOfAttempts = 5)
        {
            MaximumNumberOfAttempts = maximumNumberOfAttempts;
        }

        public int MaximumNumberOfAttempts { get; }
        
        public bool ShouldRetry(RateLimitPolicyContext context)
        {
            return context.Attempts < MaximumNumberOfAttempts;
        }
    }
}