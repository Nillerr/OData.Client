using Newtonsoft.Json;

namespace OData.Client.Json.Net
{
    /// <summary>
    /// Creates <see cref="IODataProperties{TEntity}"/> instances using an underlying
    /// <see cref="JObjectEntity{TEntity}"/>.
    /// </summary>
    public sealed class JsonNetPropertiesFactory : IODataPropertiesFactory
    {
        private readonly JsonSerializer _serializer;
        private readonly IEntitySetNameResolver _entitySetNameResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetPropertiesFactory"/> class.
        /// </summary>
        /// <param name="entitySetNameResolver">The entity set name resolver.</param>
        public JsonNetPropertiesFactory(IEntitySetNameResolver entitySetNameResolver)
        {
            _entitySetNameResolver = entitySetNameResolver;
            _serializer = JsonSerializer.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetPropertiesFactory"/> class.
        /// </summary>
        /// <param name="entitySetNameResolver">The entity set name resolver.</param>
        /// <param name="settings">The settings to be applied to the resulting <see cref="JsonSerializer"/>.</param>
        public JsonNetPropertiesFactory(IEntitySetNameResolver entitySetNameResolver, JsonSerializerSettings settings)
        {
            _entitySetNameResolver = entitySetNameResolver;
            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetPropertiesFactory"/> class.
        /// </summary>
        /// <param name="entitySetNameResolver">The entity set name resolver.</param>
        /// <param name="serializer">The serializer to use.</param>
        public JsonNetPropertiesFactory(IEntitySetNameResolver entitySetNameResolver, JsonSerializer serializer)
        {
            _entitySetNameResolver = entitySetNameResolver;
            _serializer = serializer;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IODataProperties{TEntity}"/> using an underlying
        /// <see cref="JObjectProperties{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The properties.</returns>
        public IODataProperties<TEntity> Create<TEntity>() where TEntity : IEntity
        {
            return new JObjectProperties<TEntity>(_serializer, _entitySetNameResolver);
        }
    }
}