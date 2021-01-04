using System;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataClient
    {
        IODataCollection<TEntity> Collection<TEntity>(IEntityType<TEntity> type) 
            where TEntity : IEntity;

        Task<IFindResponse<TEntity>> FindAsync<TEntity>(
            IEntityType<TEntity> type,
            ODataFindRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;
        
        Task<IFindResponse<TEntity>?> FindNextAsync<TEntity>(
            IFindResponse<TEntity> current,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task<IEntity<TEntity>?> RetrieveAsync<TEntity>(
            IEntityId<TEntity> id,
            ODataRetrieveRequest<TEntity> request,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task<EntityId<TEntity>> CreateAsync<TEntity>(
            IEntityType<TEntity> type,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task<IEntity<TEntity>> CreateRepresentationAsync<TEntity>(
            IEntityType<TEntity> type,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task UpdateAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task<IEntity<TEntity>> UpdateRepresentationAsync<TEntity>(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task DeleteAsync<TEntity>(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
            where TEntity : IEntity;

        Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

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