using System.IO;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface ISerializer
    {
        ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(Stream stream) where TEntity : IEntity;
    }
}