using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace OData.Client
{
    public sealed class SystemTextJsonSerializer : ISerializer
    {
        public async ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(Stream stream)
        {
            var response = await JsonSerializer.DeserializeAsync<FindResponse<TEntity>>(stream);
            if (response is null)
            {
                throw new SerializationException();
            }

            return response;
        }
    }
}