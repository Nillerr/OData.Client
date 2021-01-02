using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OData.Client.Newtonsoft.Json
{
    public sealed class JsonNetSerializer : ISerializer
    {
        public ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(Stream stream)
            where TEntity : IEntity
        {
            var serializer = new JsonSerializer();
            
            using var sr = new StreamReader(stream, Encoding.UTF8);
            using var jr = new JsonTextReader(sr);
            var response = serializer.Deserialize<JObjectFindResponse<TEntity>>(jr);
            if (response == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }
            
            return ValueTask.FromResult<IFindResponse<TEntity>>(response);
        }
    }
}