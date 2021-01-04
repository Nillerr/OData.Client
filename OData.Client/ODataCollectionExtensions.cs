namespace OData.Client
{
    /// <summary>
    /// Extensions for interacting with instances of <see cref="IODataCollection{TEntity}"/>.
    /// </summary>
    public static class ODataCollectionExtensions
    {
        /// <summary>
        /// A synonym for calling <see cref="IODataQuery{TEntity}.Filter"/> on a query returned by <see cref="IODataCollection{TEntity}.Find"/>.
        /// </summary>
        /// <param name="collection">The OData collection.</param>
        /// <param name="filter">The filter.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>A query builder for querying the OData API for the entities in this collection.</returns>
        /// <seealso cref="Where{TEntity}"/>
        public static IODataQuery<TEntity> Find<TEntity>(
            this IODataCollection<TEntity> collection,
            ODataFilter<TEntity> filter
        )
            where TEntity : IEntity
        {
            return collection.Find().Filter(filter);
        }

        /// <summary>
        /// A synonym for calling <see cref="IODataQuery{TEntity}.Filter"/> on a query returned by <see cref="IODataCollection{TEntity}.Find"/>.
        /// </summary>
        /// <param name="collection">The OData collection.</param>
        /// <param name="filter">The filter.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>A query builder for querying the OData API for the entities in this collection.</returns>
        /// <seealso cref="Find{TEntity}"/>
        public static IODataQuery<TEntity> Where<TEntity>(
            this IODataCollection<TEntity> collection,
            ODataFilter<TEntity> filter
        )
            where TEntity : IEntity
        {
            return collection.Find().Filter(filter);
        }
    }
}