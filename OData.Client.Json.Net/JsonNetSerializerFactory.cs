using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetSerializerFactory : ISerializerFactory
    {
        private readonly ConcurrentDictionary<Type, object> _serializerCache = new();

        private readonly JsonSerializerFactory _serializerFactory;

        public JsonNetSerializerFactory()
        {
            var settings = new JsonSerializerSettings();
            _serializerFactory = new JsonSerializerFactory(settings);
        }
        
        public JsonNetSerializerFactory(JsonSerializerSettings settings)
        {
            _serializerFactory = new JsonSerializerFactory(settings);
        }

        public ISerializer<TEntity> CreateSerializer<TEntity>(IEntityName<TEntity> entityName)
            where TEntity : IEntity
        {
            return (ISerializer<TEntity>) _serializerCache.GetOrAdd(typeof(TEntity), CreateSerializer, entityName);
        }

        private ISerializer<TEntity> CreateSerializer<TEntity>(Type entityType, IEntityName<TEntity> name)
            where TEntity : IEntity
        {
            return new JsonNetSerializer<TEntity>(name, _serializerFactory);
        }
    }
}