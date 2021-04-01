using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Namotion.Reflection;
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
        public IEntityId<TEntity> Id(IProperty<TEntity, IEntityId<TEntity>> property)
        {
            var propertyAsGuid = new Property<TEntity, Guid>(property.SelectableName);
            var value = this.Value(propertyAsGuid);
            var entityId = _entityType.Id(value);
            return entityId;
        }

        /// <inheritdoc />
        public bool Contains(IProperty<TEntity> property)
        {
            return _root[property.SelectableName] != null;
        }

        /// <inheritdoc />
        public bool TryGetValue<TValue>(IProperty<TEntity, TValue> property, [MaybeNullWhen(false)] out TValue value)
        {
            if (property.ValueType.IsEntityId())
            {
                throw new ArgumentException($"The property '{property.SelectableName}' is an entity id, and as such must use the '{nameof(JObjectEntity<TEntity>)}.{nameof(Id)}' method to retrieve the value.", nameof(property));
            }
            
            if (_root.TryGetValue(property.SelectableName, out var token))
            {
                var reader = new JTokenReader(token);
                value = _serializer.Deserialize<TValue>(reader);
                
                if (value is null && typeof(TValue).ToContextualType().Nullability == Nullability.NotNullable)
                {
                    throw new ODataNullValueException($"The value in the entity for required property '{property.SelectableName}' was null.", property);
                }
                
#pragma warning disable 8762
                return true;
#pragma warning restore 8762
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        public bool TryGetEntity<TOther>(IOptionalRef<TEntity, TOther> property, IEntityType<TOther> other, out IEntity<TOther> entity) where TOther : IEntity
        {
            var propertyName = EntityPropertyName(property, other);
            if (_root.TryGetValue(propertyName, out var token))
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

            var propertyName = EntityPropertyName(property, other);
            throw new JsonSerializationException($"A property '{propertyName}' could not be found.");
        }

        private string EntityPropertyName<TOther>(IRef<TEntity, TOther> property, IEntityType<TOther> other) where TOther : IEntity
        {
            var name = typeof(TOther) == typeof(IEntity)
                ? $"{property.ExpandableName}_{other.Name}"
                : property.ExpandableName;
            
            return name;
        }

        /// <inheritdoc />
        public bool TryGetEntities<TOther>(
            IRefs<TEntity, TOther> property,
            IEntityType<TOther> other,
            out IEnumerable<IEntity<TOther>> entities
        ) 
            where TOther : IEntity
        {
            if (_root.TryGetValue(property.SelectableName, out var token))
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

            throw new JsonSerializationException($"A property '{property.SelectableName}' could not be found.");
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
            var propertyName = property.ValueName;
            return _root[propertyName] != null;
        }

        /// <inheritdoc />
        public bool TryGetReference<TOther>(
            IOptionalRef<TEntity, TOther> property,
            IEntityType<TOther> other,
            out IEntityId<TOther> id
        ) where TOther : IEntity
        {
            var propertyName = property.ValueName;
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
            
            throw new JsonSerializationException($"A property '{property.SelectableName}' could not be found.");
        }

        /// <inheritdoc cref="IEntity" />
        public override string ToString()
        {
            using var stringWriter = new StringWriter();
            using var jsonTextWriter = new JsonTextWriter(stringWriter);
            jsonTextWriter.Formatting = Formatting.Indented;
            
            _serializer.Serialize(jsonTextWriter, _root);

            return stringWriter.ToString();
        }
    }
}