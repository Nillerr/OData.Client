using System.IO;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface ISerializer
    {
        ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(
            Stream stream,
            ODataFindRequest<TEntity> request
        ) where TEntity : IEntity;

        ValueTask<IEntity<TEntity>> DeserializeEntityAsync<TEntity>(Stream stream) where TEntity : IEntity;
    }
}