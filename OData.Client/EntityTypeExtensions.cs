using System;

namespace OData.Client
{
    /// <summary>
    /// Extensions for creating objects using a <see cref="IEntityType{TEntity}"/> instance.
    /// </summary>
    public static class EntityTypeExtensions
    {
        /// <summary>
        /// Creates an entity id from a <see cref="Guid"/>.
        /// </summary>
        /// <param name="type">The type of entity to create an id for.</param>
        /// <param name="id">The guid to use as the id.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>An entity id for the specified entity with the specified <see cref="Guid"/>.</returns>
        public static EntityId<TEntity> Id<TEntity>(this IEntityType<TEntity> type, Guid id) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(id, type);
        }
        
        /// <summary>
        /// Converts the string representation of a <see cref="Guid"/> to an entity id.
        /// </summary>
        /// <param name="type">The type of entity to create an id for.</param>
        /// <param name="input">The string to convert.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>An entity id for the specified entity with the specified <see cref="Guid"/>.</returns>
        public static EntityId<TEntity> ParseId<TEntity>(this IEntityType<TEntity> type, string input) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(Guid.Parse(input), type);
        }
    }
}