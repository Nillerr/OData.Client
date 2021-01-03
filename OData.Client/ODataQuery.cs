using System;
using System.Collections.Generic;
using System.Threading;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataQuery<TEntity> : IODataQuery<TEntity>, IODataOrderedQuery<TEntity> where TEntity : IEntity
    {
        private readonly List<IProperty<TEntity>> _selection = new();
        private readonly List<ODataExpansion<TEntity>> _expansions = new();
        private readonly List<Sorting<TEntity>> _sorting = new();
        
        private ODataFilter<TEntity>? _filter;

        private int? _maxPageSize;

        private readonly IODataClient _oDataClient;
        private readonly IValueFormatter _valueFormatter;

        public ODataQuery(IEntityName<TEntity> entityName, IODataClient oDataClient, IValueFormatter valueFormatter)
        {
            EntityName = entityName;
            _oDataClient = oDataClient;
            _valueFormatter = valueFormatter;
        }

        public IEntityName<TEntity> EntityName { get; }

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

        public IODataQuery<TEntity> Expand<TOther>(IRef<TEntity, TOther> property) where TOther : IEntity
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }

        public IODataQuery<TEntity> Expand<TOther>(IRefs<TEntity, TOther> property) where TOther : IEntity
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }

        public IODataOrderedQuery<TEntity> OrderBy<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            _sorting.Clear();
            SortBy(property, SortDirection.Ascending);
            return this;
        }

        public IODataOrderedQuery<TEntity> OrderByDescending<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            _sorting.Clear();
            SortBy(property, SortDirection.Descending);
            return this;
        }

        public IODataOrderedQuery<TEntity> ThenBy<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            SortBy(property, SortDirection.Ascending);
            return this;
        }

        public IODataOrderedQuery<TEntity> ThenByDescending<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            SortBy(property, SortDirection.Descending);
            return this;
        }

        private void SortBy(IProperty<TEntity> property, SortDirection direction)
        {
            _sorting.Add(new Sorting<TEntity>(property, direction));
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
            var request = CreateFindRequest();

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

        private ODataFindRequest<TEntity> CreateFindRequest()
        {
            var request = new ODataFindRequest<TEntity>(_filter, _selection, _expansions, _sorting, _maxPageSize);
            return request;
        }

        public override string ToString()
        {
            var request = CreateFindRequest();
            var queryString = request.ToQueryString(_valueFormatter, QueryStringFormatting.None);

            return queryString;
        }
    }
}