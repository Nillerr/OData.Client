using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetPropertiesFactory : IODataPropertiesFactory
    {
        private readonly IList<JsonConverter> _converters;

        public JsonNetPropertiesFactory()
        {
            _converters = JsonSerializer.CreateDefault().Converters;
        }
        
        public JsonNetPropertiesFactory(JsonSerializerSettings settings)
        {
            _converters = settings.Converters.ToList();
        }
        
        public JsonNetPropertiesFactory(IList<JsonConverter> converters)
        {
            _converters = converters;
        }

        public IODataProperties<TEntity> Create<TEntity>() where TEntity : IEntity
        {
            return new JObjectProperties<TEntity>(_converters);
        }
    }
}