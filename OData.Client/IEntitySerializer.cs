using System.IO;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// Serializes JSON to entities.
    /// </summary>
    public interface IEntitySerializer
    {
        /// <summary>
        /// Serializes JSON from the UTF-8 encoded <see cref="Stream"/> to an instance of
        /// <see cref="IFindResponse{TEntity}"/>.
        /// </summary>
        /// <param name="stream">The UTF-8 encoded stream.</param>
        /// <param name="entityType">The entity type.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The deserialized find response.</returns>
        ValueTask<IFindResponse<TEntity>> DeserializeFindResponseAsync<TEntity>(
            Stream stream,
            IEntityType<TEntity> entityType
        ) where TEntity : IEntity;

        /// <summary>
        /// Serializes JSON from the UTF-8 encoded <see cref="Stream"/> to an instance of
        /// <see cref="IEntity{TEntity}"/>.
        /// </summary>
        /// <param name="stream">The UTF-8 encoded stream.</param>
        /// <param name="entityType">The entity type.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The deserialized entity.</returns>
        ValueTask<IEntity<TEntity>> DeserializeEntityAsync<TEntity>(
            Stream stream,
            IEntityType<TEntity> entityType
        ) where TEntity : IEntity;
    }
}