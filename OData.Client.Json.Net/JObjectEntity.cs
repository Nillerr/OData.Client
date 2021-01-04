using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    /// <summary>
    /// A collection of properties retrieved for an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public sealed class JObjectEntity<TEntity> : IEntity<TEntity>
        where TEntity : IEntity
    {
        private readonly IEntityType<TEntity> _entityType;
        private readonly JObject _root;
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JObjectEntity{TEntity}"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="root">The object containing the entity properties.</param>
        /// <param name="serializer">The serializer to deserialize values with.</param>
        public JObjectEntity(IEntityType<TEntity> entityType, JObject root, JsonSerializer serializer)
        {
            _entityType = entityType;
            _root = root;
            _serializer = serializer;
        }

        /// <inheritdoc />
        public IEntityId<TEntity> Id(IRequired<TEntity, IEntityId<TEntity>> property)
        {
            var propertyAsGuid = new Required<TEntity, Guid>(property.Name);
            var value = this.Value(propertyAsGuid);
            var entityId = _entityType.Id(value);
            return entityId;
        }

        /// <inheritdoc />
        public bool Contains(IProperty<TEntity> property)
        {
            return _root[property.Name] != null;
        }

        /// <inheritdoc />
        public bool TryGetValue<TValue>(IOptional<TEntity, TValue> property, out TValue? value) where TValue : notnull
        {
            if (property.ValueType.IsEntityId())
            {
                throw new ArgumentException($"The property '{property.Name}' is an entity id, and as such must use the '{nameof(JObjectEntity<TEntity>)}.{nameof(Id)}' method to retrieve the value.", nameof(property));
            }
            
            if (_root.TryGetValue(property.Name, out var token))
            {
                var reader = new JTokenReader(token);
                value = _serializer.Deserialize<TValue>(reader);
                return true;
            }

            value = default!;
            return false;
        }

        /// <inheritdoc />
        public TValue? Value<TValue>(IOptional<TEntity, TValue> property) where TValue : notnull
        {
            if (TryGetValue(property, out var value))
            {
                return value;
            }
            
            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        /// <inheritdoc />
        public bool TryGetEntity<TOther>(IOptionalRef<TEntity, TOther> property, IEntityType<TOther> other, out IEntity<TOther> entity) where TOther : IEntity
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                var otherRoot = token.Value<JObject>();
                entity = new JObjectEntity<TOther>(other, otherRoot, _serializer);
                return true;
            }

            entity = default!;
            return false;
        }

        /// <inheritdoc />
        public IEntity<TOther> Entity<TOther>(IOptionalRef<TEntity, TOther> property, IEntityType<TOther> other) where TOther : IEntity
        {
            if (TryGetEntity(property, other, out var entity))
            {
                return entity;
            }
            
            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        /// <inheritdoc />
        public bool TryGetEntities<TOther>(
            IRefs<TEntity, TOther> property,
            IEntityType<TOther> other,
            out IEnumerable<IEntity<TOther>> entities
        ) 
            where TOther : IEntity
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                var roots = _root.Value<JArray>(token);
                entities = EntitiesFrom<TOther>(roots, other);
                return true;
            }

            entities = Array.Empty<IEntity<TOther>>();
            return false;
        }

        /// <inheritdoc />
        public IEnumerable<IEntity<TOther>> Entities<TOther>(
            IRefs<TEntity, TOther> property,
            IEntityType<TOther> other
        ) where TOther : IEntity
        {
            if (TryGetEntities(property, other, out var entities))
            {
                return entities;
            }

            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        private IEnumerable<IEntity<TOther>> EntitiesFrom<TOther>(JArray roots, IEntityType<TOther> other)
            where TOther : IEntity
        {
            foreach (var root in roots.Children<JObject>())
            {
                yield return new JObjectEntity<TOther>(other, root, _serializer);
            }
        }

        /// <inheritdoc />
        public bool ContainsReference<TOther>(IRef<TEntity, TOther> property) where TOther : IEntity
        {
            var propertyName = property.ValueName();
            return _root[propertyName] != null;
        }

        /// <inheritdoc />
        public bool TryGetReference<TOther>(
            IOptionalRef<TEntity, TOther> property,
            IEntityType<TOther> other,
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

        /// <inheritdoc />
        public IEntityId<TOther> Reference<TOther>(IOptionalRef<TEntity, TOther> property, IEntityType<TOther> other) 
            where TOther : IEntity
        {
            if (TryGetReference(property, other, out var reference))
            {
                return reference;
            }
            
            throw new JsonSerializationException($"A property '{property.Name}' could not be found.");
        }

        /// <inheritdoc />
        public string ToJson(Formatting formatting)
        {
            using var stringWriter = new StringWriter();
            using var jsonTextWriter = new JsonTextWriter(stringWriter);
            jsonTextWriter.Formatting = (Newtonsoft.Json.Formatting) formatting;
            
            _serializer.Serialize(jsonTextWriter, _root);

            return stringWriter.ToString();
        }
    }
}