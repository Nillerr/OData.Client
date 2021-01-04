namespace OData.Client
{
    /// <summary>
    /// A selection builder.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IODataSelection<in TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Specifies a property to select.
        /// </summary>
        /// <param name="property">The property to select.</param>
        /// <returns>This selection builder.</returns>
        IODataSelection<TEntity> Select(IProperty<TEntity> property);

        /// <summary>
        /// Specifies a property to expand.
        /// </summary>
        /// <param name="property">The properties to expand.</param>
        /// <returns>This selection builder.</returns>
        IODataSelection<TEntity> Expand(IRefProperty<TEntity> property);
    }
}