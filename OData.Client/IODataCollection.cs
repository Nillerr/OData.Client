using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataCollection<TEntity> where TEntity : IEntity
    {
        IODataQuery<TEntity> Find();

        Task<IEntity<TEntity>?> RetrieveAsync(IEntityId<TEntity> id);
        Task<IEntity<TEntity>?> RetrieveAsync(IEntityId<TEntity> id, Action<IODataSelection<TEntity>> selection);

        Task<IEntityId<TEntity>> CreateAsync(
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
            IProperty<TEntity, TOther?> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;

        Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;

        Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, TOther?> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;

        Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        ) 
            where TOther : IEntity;
    }
}