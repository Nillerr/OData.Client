using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// Extension methods on <see cref="IODataQuery{TEntity}"/>.
    /// </summary>
    public static class ODataQueryExtensions
    {
        /// <summary>
        /// Returns an array of all entities matching the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>An array of all matching entities.</returns>
        /// <remarks>
        /// It is recommended that you use the <see cref="IAsyncEnumerable{T}"/> implementation of
        /// <see cref="IODataQuery{TEntity}"/> for queries you expect to a large number of results, as this method will
        /// collect all results into one big array by fetching all pages one after another, before returning.
        /// </remarks>
        public static async Task<IEntity<TEntity>[]> ToArrayAsync<TEntity>(
            this IODataQuery<TEntity> query,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var list = await query.ToListAsync(cancellationToken);
            return list.ToArray();
        }

        /// <summary>
        /// Returns a list of all entities matching the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>A list of all matching entities.</returns>
        /// <remarks>
        /// It is recommended that you use the <see cref="IAsyncEnumerable{T}"/> implementation of
        /// <see cref="IODataQuery{TEntity}"/> for queries you expect to a large number of results, as this method will
        /// collect all results into one big <see cref="List{T}"/> by fetching all pages one after another, before
        /// returning.
        /// </remarks>
        public static async Task<List<IEntity<TEntity>>> ToListAsync<TEntity>(
            this IODataQuery<TEntity> query,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var entities = new List<IEntity<TEntity>>();

            await foreach (var entity in query.WithCancellation(cancellationToken))
            {
                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// Specifies a filter, appending it to any existing filters.
        /// </summary>
        /// <remarks>
        /// Subsequent calls to filter will AND the new filter with the existing filter.
        /// </remarks>
        /// <param name="query">The query instance.</param>
        /// <param name="filter">The filter.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The query instance.</returns>
        public static IODataQuery<TEntity> Where<TEntity>(this IODataQuery<TEntity> query, ODataFilter<TEntity> filter)
            where TEntity : IEntity
        {
            return query.Filter(filter);
        }
    }
}