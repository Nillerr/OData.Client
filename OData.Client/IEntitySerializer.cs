using System.IO;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IEntitySerializer<TEntity> where TEntity : IEntity
    {
        ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync(
            Stream stream,
            IODataFindRequest<TEntity> request
        );

        ValueTask<IEntity<TEntity>> DeserializeEntityAsync(Stream stream);
    }
}