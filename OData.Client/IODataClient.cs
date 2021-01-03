using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataClient
    {
        Task<IFindResponse<TEntity>> FindAsync<TEntity>(
            IEntityName<TEntity> name,
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

        Task<IEntityId<TEntity>> CreateAsync<TEntity>(
            IEntityName<TEntity> name,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity;

        Task<IEntity<TEntity>> CreateRepresentationAsync<TEntity>(
            IEntityName<TEntity> name,
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
            IProperty<TEntity, TOther?> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        Task AssociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, TOther?> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;

        Task DisassociateAsync<TEntity, TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
            where TOther : IEntity;
    }
}