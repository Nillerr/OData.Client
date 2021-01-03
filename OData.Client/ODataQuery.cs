using System;
using System.Collections.Generic;
using System.Threading;

namespace OData.Client
{
    public sealed class ODataQuery<TEntity> : IODataQuery<TEntity> where TEntity : IEntity
    {
        private readonly List<IProperty<TEntity>> _selection = new();
        private readonly List<ODataExpansion<TEntity>> _expansions = new();
        
        private ODataFilter<TEntity>? _filter;

        private int? _maxPageSize;

        private readonly IODataClient _oDataClient;

        public ODataQuery(EntityName<TEntity> entityName, IODataClient oDataClient)
        {
            EntityName = entityName;
            _oDataClient = oDataClient;
        }

        public EntityName<TEntity> EntityName { get; }

        public IODataQuery<TEntity> Filter(ODataFilter<TEntity> filter)
        {
            if (_filter.HasValue)
            {
                _filter = _filter.Value & filter;
            }
            else
            {
                _filter = filter;
            }
            
            return this;
        }

        public IODataQuery<TEntity> Select(IProperty<TEntity> property)
        {
            _selection.Add(property);
            return this;
        }

        public IODataQuery<TEntity> Select(params IProperty<TEntity>[] properties)
        {
            _selection.AddRange(properties);
            return this;
        }

        public IODataQuery<TEntity> Expand<TOther>(IProperty<TEntity, TOther?> property) where TOther : IEntity
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }

        public IODataQuery<TEntity> MaxPageSize(int? maxPageSize)
        {
            _maxPageSize = maxPageSize;
            return this;
        }

        public async IAsyncEnumerator<IEntity<TEntity>> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        )
        {
            var request = new ODataFindRequest<TEntity>(_filter, _selection, _expansions, _maxPageSize);

            const int maxIterations = 5;
            var iteration = 0;
            
            IFindResponse<TEntity>? response = await _oDataClient.FindAsync(EntityName, request, cancellationToken);
            while (response != null && iteration < maxIterations)
            {
                foreach (var entity in response.Value)
                {
                    yield return entity;
                }

                response = await _oDataClient.FindNextAsync(response, cancellationToken);
                iteration++;
            }
        }
    }
}