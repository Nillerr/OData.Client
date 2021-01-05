using System;

namespace OData.Client
{
    /// <summary>
    /// A typed id of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this id is for.</typeparam>
    public interface IEntityId<out TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The id of the entity.
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// The type of entity this id is for.
        /// </summary>
        IEntityType<TEntity> Type { get; }
    }
}