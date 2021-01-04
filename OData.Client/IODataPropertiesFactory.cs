namespace OData.Client
{
    /// <summary>
    /// Creates <see cref="IODataProperties{TEntity}"/> instances.
    /// </summary>
    public interface IODataPropertiesFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IODataProperties{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The properties.</returns>
        IODataProperties<TEntity> Create<TEntity>() where TEntity : IEntity;
    }
}