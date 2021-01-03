using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public sealed class JsonSerializerFactory
    {
        private readonly ConcurrentDictionary<Type, JsonSerializer> _serializerCache = new();
        
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonSerializerFactory()
        {
            _serializerSettings = new JsonSerializerSettings();
        }
        
        public JsonSerializerFactory(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }

        public JsonSerializer CreateSerializer<TEntity>(IEntityName<TEntity> name) where TEntity : IEntity
        {
            return _serializerCache.GetOrAdd(typeof(TEntity), CreateJsonSerializer, name);
        }

        /// <summary>
        /// Creates a <see cref="JsonSerializer"/> with an <see cref="EntityIdConverter{TEntity}"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entityName"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private JsonSerializer CreateJsonSerializer<TEntity>(Type type, IEntityName<TEntity> entityName)
            where TEntity : IEntity
        {
            var converter = new EntityIdConverter<TEntity>(entityName);
            _serializerSettings.Converters.Add(converter);
            var serializer = JsonSerializer.Create(_serializerSettings);
            _serializerSettings.Converters.Remove(converter);
            return serializer;
        }
    }
}