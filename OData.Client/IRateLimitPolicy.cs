namespace OData.Client
{
    public interface IRateLimitPolicy
    {
        bool ShouldRetry(RateLimitPolicyContext context);
    }
}