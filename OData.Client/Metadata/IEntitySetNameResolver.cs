using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IEntitySetNameResolver
    {
        Task<string> EntitySetNameAsync<TEntity>(
            ODataMetadataContext<TEntity> context,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;
    }
}