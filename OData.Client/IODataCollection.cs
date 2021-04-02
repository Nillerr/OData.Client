using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// A specialized client for interacting with the OData API of the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this collection performs operations on.</typeparam>
    public interface IODataCollection<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Creates a query builder for querying the entities in this collection.
        /// </summary>
        /// <returns>A query builder or querying the entities in this collection.</returns>
        IODataQuery<TEntity> Find();

        /// <summary>
        /// Retrieves the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity with the specified id if it exists; otherwise <see langword="null"/>.</returns>
        Task<IEntity<TEntity>?> RetrieveAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the selection specified by <paramref name="selection"/> from the entity specified by
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <param name="selection">The selection of properties to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity with the specified id if it exists; otherwise <see langword="null"/>.</returns>
        Task<IEntity<TEntity>?> RetrieveAsync(
            IEntityId<TEntity> id,
            Action<IODataSelection<TEntity>> selection,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new entity with the specified properties.
        /// </summary>
        /// <param name="props">The properties to create the entity with.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The id of the created entity.</returns>
        /// <remarks>
        /// You can specify the id of the entity on creation by setting it in the specified properties. If an entity
        /// with the specified id already exists, a <see cref="HttpRequestException"/> will be thrown with the status
        /// code <see cref="HttpStatusCode.Conflict"/>.
        /// </remarks>
        Task<EntityId<TEntity>> CreateAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Creates a new entity with the specified properties, returning the entire entity after creation.
        /// </summary>
        /// <param name="props">The properties to create the entity with.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The created entity.</returns>
        /// <remarks>
        /// You can specify the id of the entity on creation by setting it in the specified properties. If an entity
        /// with the specified id already exists, a <see cref="HttpRequestException"/> will be thrown with the status
        /// code <see cref="HttpStatusCode.Conflict"/>.   
        /// </remarks>
        Task<IEntity<TEntity>> CreateRepresentationAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates the existing entity specified by <paramref name="id"/>, setting the specified properties.
        /// </summary>
        /// <param name="id">The id of the entity to update.</param>
        /// <param name="props">The properties of the entity to change.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// If the entity does not exist, a <see cref="HttpRequestException"/> will be thrown with the status code
        /// <see cref="HttpStatusCode.NotFound"/>.
        /// </remarks>
        Task UpdateAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Updates the existing entity specified by <paramref name="id"/>, setting the specified properties, and
        /// returning the entire entity after updating it.
        /// </summary>
        /// <param name="id">The id of the entity to update.</param>
        /// <param name="props">The properties of the entity to change.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated entity.</returns>
        /// <remarks>
        /// If the entity does not exist, a <see cref="HttpRequestException"/> will be thrown with the status code
        /// <see cref="HttpStatusCode.NotFound"/>.
        /// </remarks>
        Task<IEntity<TEntity>> UpdateRepresentationAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// If the entity does not exist, a <see cref="HttpRequestException"/> will be thrown with the status code
        /// <see cref="HttpStatusCode.NotFound"/>.
        /// </remarks>
        Task DeleteAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Associates the entity specified by <paramref name="otherId"/> with the specified single-valued navigation
        /// <paramref name="property"/> of the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to associate another entity with.</param>
        /// <param name="property">The property on the entity specified by <paramref name="id"/>, which the id specified by <paramref name="otherId"/> will be linked to.</param>
        /// <param name="otherId">The id of entity to associate with the entity specified by <paramref name="id"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TOther">The type of entity to create an association to.</typeparam>
        Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity;

        /// <summary>
        /// Removes the association on the specified single-valued navigation <paramref name="property"/> of the entity
        /// specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to remove an association from.</param>
        /// <param name="property">The property on the entity to remove an associated from.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TOther">The type of entity to remove an association to.</typeparam>
        Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity;

        /// <summary>
        /// Associates the entity specified by <paramref name="otherId"/> with the specified collection-valued
        /// navigation <paramref name="property"/> of the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to associate another entity with.</param>
        /// <param name="property">The property on the entity specified by <paramref name="id"/>, which the id specified by <paramref name="otherId"/> will be added to.</param>
        /// <param name="otherId">The id of entity to associate with the entity specified by <paramref name="id"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TOther">The type of entity to create an association to.</typeparam>
        Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity;

        /// <summary>
        /// Removes the entity specified by <paramref name="otherId"/> from the associated on the specified
        /// collection-valued navigation <paramref name="property"/> of the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to remove an association from.</param>
        /// <param name="property">The property on the entity to remove an associated from.</param>
        /// <param name="otherId">The id of entity to remove the associated to.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TOther">The type of entity to remove an association to.</typeparam>
        Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity;
    }
}