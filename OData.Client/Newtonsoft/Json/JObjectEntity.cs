using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Newtonsoft.Json
{
    public sealed class JObjectEntity<TEntity> : IEntity<TEntity> where TEntity : IEntity
    {
        [JsonExtensionData]
        private JObject Root { get; set; } = null!;

        public bool Contains(IProperty<TEntity> property)
        {
            return Root[property.Name] != null;
        }

        public bool TryGetValue<TValue>(Property<TEntity, TValue> property, out TValue value)
        {
            if (Root.TryGetValue(property.Name, out var token))
            {
                value = token.Value<TValue>();
                return true;
            }

            value = default!;
            return false;
        }

        public TValue Value<TValue>(Property<TEntity, TValue> property)
        {
            return Root.Value<TValue>(property.Name);
        }

        public string ToJson()
        {
            return Root.ToString();
        }
    }
}