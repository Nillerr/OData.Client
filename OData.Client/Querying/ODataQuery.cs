using System;
using System.Collections.Generic;
using System.Threading;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    /// <inheritdoc cref="IODataQuery{TEntity}" />
    public sealed class ODataQuery<TEntity> : IODataQuery<TEntity>, IODataOrderedQuery<TEntity>
        where TEntity : IEntity
    {
        private readonly List<IProperty<TEntity>> _selection = new();
        private readonly List<ODataExpansion<TEntity>> _expansions = new();
        private readonly List<Sorting<TEntity>> _sorting = new();
        
        private ODataFilter<TEntity>? _filter;

        private int? _maxPageSize;
        
        private readonly IEntityType<TEntity> _entityType;
        private readonly IODataClient _oDataClient;
        private readonly IValueFormatter _valueFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataQuery{TEntity}"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="oDataClient">The OData client.</param>
        /// <param name="valueFormatter">The value formatter.</param>
        public ODataQuery(IEntityType<TEntity> entityType, IODataClient oDataClient, IValueFormatter valueFormatter)
        {
            _entityType = entityType;
            _oDataClient = oDataClient;
            _valueFormatter = valueFormatter;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IODataQuery<TEntity> Select(IProperty<TEntity> property)
        {
            _selection.Add(property);
            return this;
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> Select(params IProperty<TEntity>[] properties)
        {
            _selection.AddRange(properties);
            return this;
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> Expand<TOther>(IRef<TEntity, TOther> property) where TOther : IEntity
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> Expand<TOther>(IRefs<TEntity, TOther> property) where TOther : IEntity
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> OrderBy<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            _sorting.Clear();
            SortBy(property, SortDirection.Ascending);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> OrderByDescending<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            _sorting.Clear();
            SortBy(property, SortDirection.Descending);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> ThenBy<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            SortBy(property, SortDirection.Ascending);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> ThenByDescending<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable
        {
            SortBy(property, SortDirection.Descending);
            return this;
        }

        private void SortBy(IProperty<TEntity> property, SortDirection direction)
        {
            _sorting.Add(new Sorting<TEntity>(property, direction));
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> MaxPageSize(int? maxPageSize)
        {
            _maxPageSize = maxPageSize;
            return this;
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IEntity<TEntity>> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        )
        {
            var request = CreateFindRequest();

            IFindResponse<TEntity>? response = await _oDataClient.FindAsync(_entityType, request, cancellationToken);
            while (response != null)
            {
                foreach (var entity in response.Value)
                {
                    yield return entity;
                }
                
                response = await _oDataClient.FindNextAsync(response, cancellationToken);
            }
        }

        private ODataFindRequest<TEntity> CreateFindRequest()
        {
            var request = new ODataFindRequest<TEntity>(_filter, _selection, _expansions, _sorting, _maxPageSize);
            return request;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var request = CreateFindRequest();
            var queryString = request.ToQueryString(_valueFormatter, QueryStringFormatting.None);

            return $"{Environment.NewLine}Expression: {queryString}{Environment.NewLine}MaxPageSize: {_maxPageSize}";
        }
    }
}