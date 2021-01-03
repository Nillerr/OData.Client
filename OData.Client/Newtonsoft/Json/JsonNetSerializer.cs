using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Newtonsoft.Json
{
    public sealed class JsonNetSerializer : ISerializer
    {
        private readonly JsonSerializer _serializer;

        public JsonNetSerializer()
        {
            _serializer = JsonSerializer.CreateDefault();
        }

        public JsonNetSerializer(JsonSerializer serializer)
        {
            _serializer = serializer;
        }
        
        public ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(
            Stream stream,
            ODataFindRequest<TEntity> request
        )
            where TEntity : IEntity
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var response = _serializer.Deserialize<JObjectFindResponse<TEntity>>(jsonReader);
            if (response == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }

            response.Request = request;
            return ValueTask.FromResult<IFindResponse<TEntity>>(response);
        }

        public ValueTask<IEntity<TEntity>> DeserializeEntityAsync<TEntity>(Stream stream) where TEntity : IEntity
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);
            using var jsonReader = new JsonTextReader(streamReader);
            
            var root = _serializer.Deserialize<JObject>(jsonReader);
            if (root == null)
            {
                throw new JsonSerializationException("Could not deserialize response to JObject");
            }

            var entity = root.ToEntity<TEntity>();
            return ValueTask.FromResult<IEntity<TEntity>>(entity);
        }
    }
}