using System.IO;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IEntitySerializer
    {
        ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(
            Stream stream,
            IODataFindRequest<TEntity> request
        ) where TEntity : IEntity;

        ValueTask<IEntity<TEntity>> DeserializeEntityAsync<TEntity>(Stream stream) where TEntity : IEntity;
    }
}