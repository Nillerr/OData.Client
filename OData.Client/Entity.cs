using Newtonsoft.Json.Linq;

namespace OData.Client
{
    public sealed class JObjectEntity<TEntity> where TEntity : IEntity
    {
        private readonly JObject _root;

        public JObjectEntity(JObject root)
        {
            _root = root;
        }

        /// <summary>
        /// Tries to get the value with the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns><see langword="true"/> if a value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue<TValue>(Property<TEntity, TValue> property, out TValue value)
        {
            if (_root.TryGetValue(property.Name, out var token))
            {
                value = token.Value<TValue>();
                return true;
            }

            value = default!;
            return false;
        }

        /// <summary>
        /// Gets the value with the specified <paramref name="property"/> converted to the specified type.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public TValue Value<TValue>(Property<TEntity, TValue> property)
        {
            return _root.Value<TValue>(property.Name);
        }
    }
}