using System.IO;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface ISerializer<TEntity> where TEntity : IEntity
    {
        ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync(
            Stream stream,
            ODataFindRequest<TEntity> request
        );

        ValueTask<IEntity<TEntity>> DeserializeEntityAsync(Stream stream);
    }
}