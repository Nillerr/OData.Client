using System.Collections.Generic;
using Newtonsoft.Json;

namespace OData.Client.Newtonsoft.Json
{
    public sealed class JsonNetPropertiesFactory : IODataPropertiesFactory
    {
        private readonly IList<JsonConverter> _converters;

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