using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    public sealed class JsonNetEntitySerializerFactory : IEntitySerializerFactory
    {
        private readonly ConcurrentDictionary<Type, object> _serializerCache = new();

        private readonly IJsonSerializerFactory _serializerFactory;

        public JsonNetEntitySerializerFactory()
        {
            var settings = new JsonSerializerSettings();
            _serializerFactory = new JsonSerializerFactory(settings);
        }
        
        public JsonNetEntitySerializerFactory(JsonSerializerSettings settings)
        {
            _serializerFactory = new JsonSerializerFactory(settings);
        }
        
        public JsonNetEntitySerializerFactory(IJsonSerializerFactory serializerFactory)
        {
            _serializerFactory = serializerFactory;
        }

        public IEntitySerializer<TEntity> CreateSerializer<TEntity>(IEntityName<TEntity> entityName)
            where TEntity : IEntity
        {
            return (IEntitySerializer<TEntity>) _serializerCache.GetOrAdd(typeof(TEntity), CreateSerializer, entityName);
        }

        private IEntitySerializer<TEntity> CreateSerializer<TEntity>(Type entityType, IEntityName<TEntity> name)
            where TEntity : IEntity
        {
            return new JsonNetEntitySerializer<TEntity>(name, _serializerFactory);
        }
    }
}