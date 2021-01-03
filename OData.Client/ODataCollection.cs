using System;
using System.Threading;
using System.Threading.Tasks;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    internal sealed class ODataCollection<TEntity> : IODataCollection<TEntity> where TEntity : IEntity
    {
        private readonly IODataClient _oDataClient;
        private readonly IValueFormatter _valueFormatter;

        public ODataCollection(IEntityName<TEntity> entityName, IODataClient oDataClient, IValueFormatter valueFormatter)
        {
            EntityName = entityName;
            _oDataClient = oDataClient;
            _valueFormatter = valueFormatter;
        }

        public IEntityName<TEntity> EntityName { get; }

        public IODataQuery<TEntity> Find()
        {
            return new ODataQuery<TEntity>(EntityName, _oDataClient, _valueFormatter);
        }

        public async Task<IEntity<TEntity>?> RetrieveAsync(IEntityId<TEntity> id)
        {
            var entity = await _oDataClient.RetrieveAsync(id, new ODataRetrieveRequest<TEntity>());
            return entity;
        }

        public async Task<IEntity<TEntity>?> RetrieveAsync(
            IEntityId<TEntity> id,
            Action<IODataSelection<TEntity>> selection
        )
        {
            var request = new ODataRetrieveRequest<TEntity>();
            selection(request);

            var entity = await _oDataClient.RetrieveAsync(id, request);
            return entity;
        }

        public async Task<IEntityId<TEntity>> CreateAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            var entityId = await _oDataClient.CreateAsync(EntityName, props, cancellationToken);
            return entityId;
        }

        public async Task<IEntity<TEntity>> CreateRepresentationAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _oDataClient.CreateRepresentationAsync(EntityName, props, cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            await _oDataClient.UpdateAsync(id, props, cancellationToken);
        }

        public async Task<IEntity<TEntity>> UpdateRepresentationAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _oDataClient.UpdateRepresentationAsync(id, props, cancellationToken);
            return entity;
        }

        public async Task DeleteAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
        {
            await _oDataClient.DeleteAsync(id, cancellationToken);
        }

        public async Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity
        {
            await _oDataClient.AssociateAsync(id, property, otherId, cancellationToken);
        }

        public async Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity
        {
            await _oDataClient.DisassociateAsync(id, property, otherId, cancellationToken);
        }

        public async Task AssociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity
        {
            await _oDataClient.AssociateAsync(id, property, otherId, cancellationToken);
        }

        public async Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IRefs<TEntity, TOther> property,
            IEntityId<TOther> otherId,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity
        {
            await _oDataClient.DisassociateAsync(id, property, otherId, cancellationToken);
        }
    }
}