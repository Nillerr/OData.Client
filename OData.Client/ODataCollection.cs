using System;
using System.Threading;
using System.Threading.Tasks;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    internal sealed class ODataCollection<TEntity> : IODataCollection<TEntity>
        where TEntity : IEntity
    {
        private readonly IEntityType<TEntity> _entityType;
        private readonly IODataClient _oDataClient;
        private readonly IExpressionFormatter _expressionFormatter;

        public ODataCollection(
            IEntityType<TEntity> entityType,
            IODataClient oDataClient,
            IExpressionFormatter expressionFormatter
        )
        {
            _entityType = entityType;
            _oDataClient = oDataClient;
            _expressionFormatter = expressionFormatter;
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> Find()
        {
            return new ODataQuery<TEntity>(_entityType, _oDataClient, _expressionFormatter);
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>?> RetrieveAsync(
            IEntityId<TEntity> id,
            CancellationToken cancellationToken = default)
        {
            var entity = await _oDataClient.RetrieveAsync(id, new ODataRetrieveRequest<TEntity>(), cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>?> RetrieveAsync(
            IEntityId<TEntity> id,
            Action<IODataSelection<TEntity>> selection,
            CancellationToken cancellationToken = default
        )
        {
            var request = new ODataRetrieveRequest<TEntity>();
            selection(request);

            var entity = await _oDataClient.RetrieveAsync(id, request, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task<EntityId<TEntity>> CreateAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            var entityId = await _oDataClient.CreateAsync(_entityType, props, cancellationToken);
            return entityId;
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>> CreateRepresentationAsync(
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _oDataClient.CreateRepresentationAsync(_entityType, props, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            await _oDataClient.UpdateAsync(id, props, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEntity<TEntity>> UpdateRepresentationAsync(
            IEntityId<TEntity> id,
            Action<IODataProperties<TEntity>> props,
            CancellationToken cancellationToken = default
        )
        {
            var entity = await _oDataClient.UpdateRepresentationAsync(id, props, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default)
        {
            await _oDataClient.DeleteAsync(id, cancellationToken);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task DisassociateAsync<TOther>(
            IEntityId<TEntity> id,
            IOptionalRef<TEntity, TOther> property,
            CancellationToken cancellationToken = default
        )
            where TOther : IEntity
        {
            await _oDataClient.DisassociateAsync(id, property, cancellationToken);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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