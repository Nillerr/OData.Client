using System.Collections.Generic;

namespace OData.Client
{
    /// <summary>
    /// A request object to use when querying for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to query.</typeparam>
    public interface IODataFindRequest<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The filter to apply.
        /// </summary>
        ODataFilter<TEntity>? Filter { get; }
        
        /// <summary>
        /// The selection to apply.
        /// </summary>
        IEnumerable<IProperty<TEntity>> Selection { get; }
        
        /// <summary>
        /// The expansions to apply.
        /// </summary>
        IEnumerable<ODataExpansion<TEntity>> Expansions { get; }
        
        /// <summary>
        /// The sorting to apply.
        /// </summary>
        IEnumerable<Sorting<TEntity>> Sorting { get; }
        
        /// <summary>
        /// The maximum number of results per page.
        /// </summary>
        int? MaxPageSize { get; }
    }
}