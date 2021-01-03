using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JObjectEntity<TEntity> : IEntity<TEntity> where TEntity : IEntity
    {
        private readonly JObject _root;
        private readonly IEntityName<TEntity> _entityName;

        public JObjectEntity(JObject root, IEntityName<TEntity> entityName)
        {
            _root = root;
            _entityName = entityName;
        }

        public bool Contains(IProperty<TEntity> property)
        {
            return _root[property.Name] != null;
        }

        public bool TryGetValue<TValue>(IProperty<TEntity, TValue> property, out TValue value)
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                value = token.Value<TValue>();
                return true;
            }

            value = default!;
            return false;
        }

        public TValue Value<TValue>(IProperty<TEntity, TValue> property)
        {
            return _root.Value<TValue>(property.Name);
        }

        public bool TryGetEntity<TOther>(IProperty<TEntity, TOther?> property, IEntityName<TOther> other, out IEntity<TOther> entity) where TOther : IEntity
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                var otherRoot = token.Value<JObject>();
                entity = new JObjectEntity<TOther>(otherRoot, other);
                return true;
            }

            entity = default!;
            return false;
        }

        public IEntity<TOther> Entity<TOther>(IProperty<TEntity, TOther?> property, IEntityName<TOther> other) where TOther : IEntity
        {
            var otherRoot = _root.Value<JObject>(property.Name);
            var entity = new JObjectEntity<TOther>(otherRoot, other);
            return entity;
        }

        public bool TryGetEntities<TOther>(
            IProperty<TEntity, IEnumerable<TOther>> property,
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

        public IEnumerable<IEntity<TOther>> Entities<TOther>(IProperty<TEntity, IEnumerable<TOther>> property, IEntityName<TOther> other) where TOther : IEntity
        {
            var roots = _root.Value<JArray>(property.Name);
            var entities = EntitiesFrom(roots, other);
            return entities;
        }

        private IEnumerable<IEntity<TOther>> EntitiesFrom<TOther>(JArray roots, IEntityName<TOther> name)
            where TOther : IEntity
        {
            foreach (var root in roots.Children<JObject>())
            {
                yield return new JObjectEntity<TOther>(root, name);
            }
        }

        public IEntityId<TEntity> Value(IProperty<TEntity, IEntityId<TEntity>> property)
        {
            var stringValue = _root.Value<string>(property.Name);
            return _entityName.ParseId(stringValue);
        }

        public bool ContainsReference<TOther>(IProperty<TEntity, TOther?> property) where TOther : IEntity
        {
            var propertyName = property.ValueName();
            return _root[propertyName] != null;
        }

        public bool TryGetReference<TOther>(IProperty<TEntity, TOther?> property, out Guid id) where TOther : IEntity
        {
            var propertyName = property.ValueName();
            if (_root.TryGetValue(propertyName, out var token))
            {
                var value = token.Value<string>();
                id = Guid.Parse(value);
                return true;
            }

            id = default!;
            return false;
        }

        public Guid Reference<TOther>(IProperty<TEntity, TOther?> property) where TOther : IEntity
        {
            var propertyName = property.ValueName();
            var value = _root.Value<string>(propertyName);
            return Guid.Parse(value);
        }

        public string ToJson()
        {
            return _root.ToString();
        }
    }
}