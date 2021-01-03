using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    public sealed class JObjectEntity<TEntity> : IEntity<TEntity> where TEntity : IEntity
    {
        private readonly JObject _root;
        private readonly EntityName<TEntity> _entityName;

        public JObjectEntity(JObject root, EntityName<TEntity> entityName)
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

        public IEntityId<TEntity> Value(IProperty<TEntity, IEntityId<TEntity>> property)
        {
            var stringValue = _root.Value<string>(property.Name);
            return _entityName.ParseId(stringValue);
        }

        public string ToJson()
        {
            return _root.ToString();
        }
    }
}