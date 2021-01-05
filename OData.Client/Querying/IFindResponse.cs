using System;
using System.Collections.Generic;

namespace OData.Client
{
    /// <summary>
    /// The response returned by an OData entity query.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IFindResponse<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Name of the entity.
        /// </summary>
        IEntityType<TEntity> EntityType { get; }
        
        /// <summary>
        /// The context of the request.
        /// </summary>
        Uri Context { get; }
        
        /// <summary>
        /// The link for fetching the next page, or <see langword="null"/> if no more pages are available.
        /// </summary>
        Uri? NextLink { get; }
        
        /// <summary>
        /// The entities returned by this find request.
        /// </summary>
        IReadOnlyList<IEntity<TEntity>> Value { get; }
    }
}