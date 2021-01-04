using System;
using System.Collections.Generic;

namespace OData.Client
{
    public interface IFindResponse<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Name of the entity.
        /// </summary>
        IEntityName<TEntity> EntityName { get; }
        
        /// <summary>
        /// The context of the request.
        /// </summary>
        Uri Context { get; }
        
        /// <summary>
        /// The link to the next page if more pages are available.
        /// </summary>
        Uri? NextLink { get; }
        
        /// <summary>
        /// The entities returned by this find request.
        /// </summary>
        IReadOnlyList<IEntity<TEntity>> Value { get; }
        
        /// <summary>
        /// The original request.
        /// </summary>
        IODataFindRequest<TEntity> Request { get; }
    }
}