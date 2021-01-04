using System;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// A specialized client for interacting with the OData API, narrowed to <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this collection performs operations on.</typeparam>
    public interface IODataCollection<TEntity>
        where TEntity : IEntity
    {
        IODataQuery<TEntity> Find();

        Task<IEntity<TEntity>?> RetrieveAsync(IEntityId<TEntity> id);
        Task<IEntity<TEntity>?> RetrieveAsync(IEntityId<TEntity> id, Action<IODataSelection<TEntity>> selection);

        Task<EntityId<TEntity>> CreateAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );
        Task<IEntity<TEntity>> CreateRepresentationAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );

        Task UpdateAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );
        Task<IEntity<TEntity>> UpdateRepresentationAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default);

        Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;

        Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;

        Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;

        Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;
    }
}