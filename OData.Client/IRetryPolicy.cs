using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IRetryPolicy
    {
        Task<bool?> ShouldRetryAsync(
            RateLimitPolicyContext context,
            CancellationToken cancellationToken = default
        );
    }
}