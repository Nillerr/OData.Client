using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public interface IJsonSerializerFactory
    {
        JsonSerializer CreateSerializer<TEntity>(IEntityName<TEntity> name) where TEntity : IEntity;
    }

    public sealed class JsonSerializerFactory : IJsonSerializerFactory
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
        /// Creates a <see cref="JsonSerializer"/> with an <see cref="EntityIdConverter{TEntity}"/> for the entity this
        /// serializer is built for.
        /// </summary>
        /// <param name="type">The type of the entity.</param>
        /// <param name="entityName">The name of the entity.</param>
        /// <returns>A newly created <see cref="JsonSerializer"/>.</returns>
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