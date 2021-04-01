using System.Collections.Generic;

namespace OData.Client
{
    /// <summary>
    /// A request object to use when querying for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to query.</typeparam>
    public interface IODataFindRequest<TEntity> : IODataFindRequestHeaders<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The filter to apply.
        /// </summary>
        ODataFilter<TEntity>? Filter { get; }
        
        /// <summary>
        /// The selection to apply.
        /// </summary>
        IReadOnlyCollection<ISelectableProperty<TEntity>> Selection { get; }
        
        /// <summary>
        /// The expansions to apply.
        /// </summary>
        IReadOnlyCollection<ODataExpansion<TEntity>> Expansions { get; }
        
        /// <summary>
        /// The sorting to apply.
        /// </summary>
        IReadOnlyList<Sorting<TEntity>> Sorting { get; }
    }
}