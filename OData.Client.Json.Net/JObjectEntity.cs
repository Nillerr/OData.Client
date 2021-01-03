using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JObjectEntity<TEntity> : IEntity<TEntity> where TEntity : IEntity
    {
        private readonly JObject _root;
        private readonly JsonSerializer _serializer;
        private readonly JsonSerializerFactory _serializerFactory;

        public JObjectEntity(JObject root, JsonSerializer serializer, JsonSerializerFactory serializerFactory)
        {
            _root = root;
            _serializer = serializer;
            _serializerFactory = serializerFactory;
        }

        public bool Contains(IProperty<TEntity> property)
        {
            return _root[property.Name] != null;
        }

        public bool TryGetValue<TValue>(IOptional<TEntity, TValue> property, out TValue? value) where TValue : notnull
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                var reader = new JTokenReader(token);
                value = _serializer.Deserialize<TValue>(reader);
                return true;
            }

            value = default!;
            return false;
        }

        public TValue? Value<TValue>(IOptional<TEntity, TValue> property) where TValue : notnull
        {
            if (TryGetValue(property, out var value))
            {
                return value;
            }
            
            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        public bool TryGetEntity<TOther>(IOptionalRef<TEntity, TOther> property, IEntityName<TOther> other, out IEntity<TOther> entity) where TOther : IEntity
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                var otherRoot = token.Value<JObject>();
                
                var serializer = _serializerFactory.CreateSerializer(other);
                entity = new JObjectEntity<TOther>(otherRoot, serializer, _serializerFactory);
                
                return true;
            }

            entity = default!;
            return false;
        }

        public IEntity<TOther> Entity<TOther>(IOptionalRef<TEntity, TOther> property, IEntityName<TOther> other) where TOther : IEntity
        {
            if (TryGetEntity(property, other, out var entity))
            {
                return entity;
            }
            
            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        public bool TryGetEntities<TOther>(
            IRequired<TEntity, IEnumerable<TOther>> property,
            IEntityName<TOther> other,
            out IEnumerable<IEntity<TOther>> entities
        ) 
            where TOther : IEntity
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                var roots = _root.Value<JArray>(token);
                entities = EntitiesFrom(roots, other);
                return true;
            }

            entities = null!;
            return false;
        }

        public IEnumerable<IEntity<TOther>> Entities<TOther>(
            IRequired<TEntity, IEnumerable<TOther>> property,
            IEntityName<TOther> other
        ) where TOther : IEntity
        {
            if (TryGetEntities(property, other, out var entity))
            {
                return entity;
            }

            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        private IEnumerable<IEntity<TOther>> EntitiesFrom<TOther>(JArray roots, IEntityName<TOther> name)
            where TOther : IEntity
        {
            var serializer = _serializerFactory.CreateSerializer(name);
            foreach (var root in roots.Children<JObject>())
            {
                yield return new JObjectEntity<TOther>(root, serializer, _serializerFactory);
            }
        }

        public bool ContainsReference<TOther>(IEntityReference<TEntity, TOther> property) where TOther : IEntity
        {
            var propertyName = property.ValueName();
            return _root[propertyName] != null;
        }

        public bool TryGetReference<TOther>(
            IOptionalRef<TEntity, TOther> property,
            IEntityName<TOther> other,
            out IEntityId<TOther> id
        ) where TOther : IEntity
        {
            var propertyName = property.ValueName();
            if (_root.TryGetValue(propertyName, out var token))
            {
                var value = token.Value<string>();
                id = other.ParseId(value);
                return true;
            }

            id = default!;
            return false;
        }

        public IEntityId<TOther> Reference<TOther>(IOptionalRef<TEntity, TOther> property, IEntityName<TOther> other) 
            where TOther : IEntity
        {
            if (TryGetReference(property, other, out var reference))
            {
                return reference;
            }
            
            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        public string ToJson()
        {
            return _root.ToString();
        }
    }
}