using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A type of entity.
    /// </summary>
    /// <typeparam name="TEntity">The CLR type representing the entity.</typeparam>
    [PublicAPI]
    public interface IEntityType<[UsedImplicitly] out TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The pluralized name / endpoint of the entity, e.g. <c>"accounts"</c>, <c>"contacts"</c>.
        /// </summary>
        string Name { get; }
    }
}