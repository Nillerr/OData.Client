using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetPropertiesFactory : IODataPropertiesFactory
    {
        private readonly JsonSerializer _serializer;

        public JsonNetPropertiesFactory()
        {
            _serializer = JsonSerializer.Create();
        }
        
        public JsonNetPropertiesFactory(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }
        
        public JsonNetPropertiesFactory(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public IODataProperties<TEntity> Create<TEntity>() where TEntity : IEntity
        {
            return new JObjectProperties<TEntity>(_serializer);
        }
    }
}