using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A type of entity.
    /// </summary>
    /// <typeparam name="TEntity">The CLR type representing the entity.</typeparam>
    public interface IEntityType<[UsedImplicitly] out TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The name of the entity, e.g. <c>"account"</c>, <c>"contact"</c>.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// The name of the id property.
        /// </summary>
        string IdPropertyName { get; }
    }
}