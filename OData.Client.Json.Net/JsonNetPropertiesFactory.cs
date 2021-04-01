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

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetPropertiesFactory"/> class.
        /// </summary>
        public JsonNetPropertiesFactory()
        {
            _serializer = JsonSerializer.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetPropertiesFactory"/> class.
        /// </summary>
        /// <param name="settings">The settings to be applied to the resulting <see cref="JsonSerializer"/>.</param>
        public JsonNetPropertiesFactory(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetPropertiesFactory"/> class.
        /// </summary>
        /// <param name="serializer">The serializer to use.</param>
        public JsonNetPropertiesFactory(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IODataProperties{TEntity}"/> using an underlying
        /// <see cref="JObjectProperties{TEntity}"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The properties.</returns>
        public IODataProperties<TEntity> Create<TEntity>(ODataPropertiesFactoryContext<TEntity> context)
            where TEntity : IEntity
        {
            return new JObjectProperties<TEntity>(context.ODataClient, _serializer, context.EntitySetNameResolver, context.EntityType);
        }
    }
}