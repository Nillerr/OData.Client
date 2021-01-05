using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// An interface for an OData client.
    /// </summary>
    public interface IODataClient
    {
        /// <summary>
        /// Returns a specialized client for interacting with the OData API of the specified entity.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The specialized client for the specified entity type.</returns>
        IODataCollection<TEntity> Collection<TEntity>(IEntityType<TEntity> type) 
            where TEntity : IEntity;

        /// <summary>
        /// Returns the entities matching the request.
        /// </summary>
        /// <param name="type">The entity type/</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The entities matching the request.</returns>
        Task<IFindResponse<TEntity>> FindAsync<TEntity>(
            IEntityType<TEntity> type,
            ODataFindRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;
        
        /// <summary>
        /// Returns the next page of entities from a previous call to <see cref="FindAsync{TEntity}"/>.
        /// </summary>
        /// <param name="current">The previous returned page.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The next page of entities.</returns>
        Task<IFindResponse<TEntity>?> FindNextAsync<TEntity>(
            IFindResponse<TEntity> current,
            ODataFindNextRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        /// <summary>
        /// Retrieves the entity with the specified <paramref cref="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The entity with the specified id if it exists; otherwise <see langword="null"/>.</returns>
        Task<IEntity<TEntity>?> RetrieveAsync<TEntity>(
            IEntityId<TEntity> id,
            ODataRetrieveRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        /// <summary>
        /// Creates a new entity with the specified properties.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <param name="props">The properties to create the entity with.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The id of the created entity.</returns>
        /// <remarks>
        /// You can specify the id of the entity on creation by setting it in the specified properties. If an entity
        /// with the specified id already exists, a <see cref="HttpRequestException"/> will be thrown with the status
        /// code <see cref="HttpStatusCode.Conflict"/>.   
        /// </remarks>
        Task<EntityId<TEntity>> CreateAsync<TEntity>(
            IEntityType<TEntity> type,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        /// <summary>
        /// Creates a new entity with the specified properties, returning the entire entity after creation.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <param name="props">The properties to create the entity with.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The created entity.</returns>
        /// <remarks>
        /// You can specify the id of the entity on creation by setting it in the specified properties. If an entity
        /// with the specified id already exists, a <see cref="HttpRequestException"/> will be thrown with the status
        /// code <see cref="HttpStatusCode.Conflict"/>.   
        /// </remarks>
        Task<IEntity<TEntity>> CreateRepresentationAsync<TEntity>(
            IEntityType<TEntity> type,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        /// <summary>
        /// Updates the existing entity specified by <paramref name="id"/>, setting the specified properties.
        /// </summary>
        /// <param name="id">The id of the entity to update.</param>
        /// <param name="props">The properties of the entity to change.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <remarks>
        /// If the entity does not exist, a <see cref="HttpRequestException"/> will be thrown with the status code
        /// <see cref="HttpStatusCode.NotFound"/>.
        /// </remarks>
        Task UpdateAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        /// <summary>
        /// Updates the existing entity specified by <paramref name="id"/>, setting the specified properties, and
        /// returning the entire entity after updating it.
        /// </summary>
        /// <param name="id">The id of the entity to update.</param>
        /// <param name="props">The properties of the entity to change.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The updated entity.</returns>
        /// <remarks>
        /// If the entity does not exist, a <see cref="HttpRequestException"/> will be thrown with the status code
        /// <see cref="HttpStatusCode.NotFound"/>.
        /// </remarks>
        Task<IEntity<TEntity>> UpdateRepresentationAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        /// <summary>
        /// Deletes the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <remarks>
        /// If the entity does not exist, a <see cref="HttpRequestException"/> will be thrown with the status code
        /// <see cref="HttpStatusCode.NotFound"/>.
        /// </remarks>
        Task DeleteAsync<TEntity>(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
            where TEntity : IEntity;

        /// <summary>
        /// Associates the entity specified by <paramref name="otherId"/> with the specified single-valued navigation
        /// <paramref name="property"/> of the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to associate another entity with.</param>
        /// <param name="property">The property on the entity specified by <paramref name="id"/>, which the id specified by <paramref name="otherId"/> will be linked to.</param>
        /// <param name="otherId">The id of entity to associate with the entity specified by <paramref name="id"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity to create an association from.</typeparam>
        /// <typeparam name="TOther">The type of entity to create an association to.</typeparam>
        Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        /// <summary>
        /// Removes the association on the specified single-valued navigation <paramref name="property"/> of the entity
        /// specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to remove an association from.</param>
        /// <param name="property">The property on the entity to remove an associated from.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity to remove an association from.</typeparam>
        /// <typeparam name="TOther">The type of entity to remove an association to.</typeparam>
        Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        /// <summary>
        /// Associates the entity specified by <paramref name="otherId"/> with the specified collection-valued
        /// navigation <paramref name="property"/> of the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to associate another entity with.</param>
        /// <param name="property">The property on the entity specified by <paramref name="id"/>, which the id specified by <paramref name="otherId"/> will be added to.</param>
        /// <param name="otherId">The id of entity to associate with the entity specified by <paramref name="id"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity to create an association from.</typeparam>
        /// <typeparam name="TOther">The type of entity to create an association to.</typeparam>
        Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        /// <summary>
        /// Removes the entity specified by <paramref name="otherId"/> from the associated on the specified
        /// collection-valued navigation <paramref name="property"/> of the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to remove an association from.</param>
        /// <param name="property">The property on the entity to remove an associated from.</param>
        /// <param name="otherId">The id of entity to remove the associated to.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TEntity">The type of entity to remove an association from.</typeparam>
        /// <typeparam name="TOther">The type of entity to remove an association to.</typeparam>
        Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;
    }
}