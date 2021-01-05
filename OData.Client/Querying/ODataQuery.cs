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
        private readonly List<ISelectableProperty<TEntity>> _selection = new();
        private readonly List<ODataExpansion<TEntity>> _expansions = new();
        private readonly List<Sorting<TEntity>> _sorting = new();
        
        private ODataFilter<TEntity>? _filter;

        private int? _maxPageSize;
        private int? _limit;
        private int? _offset;
        
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
        public IODataQuery<TEntity> Select(ISelectableProperty<TEntity> property)
        {
            _selection.Add(property);
            return this;
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> Select(params ISelectableProperty<TEntity>[] properties)
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
        public IODataOrderedQuery<TEntity> OrderBy<TValue>(ISortableProperty<TEntity, TValue> property) where TValue : IComparable
        {
            _sorting.Clear();
            SortBy(property, SortDirection.Ascending);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> OrderByDescending<TValue>(ISortableProperty<TEntity, TValue> property) where TValue : IComparable
        {
            _sorting.Clear();
            SortBy(property, SortDirection.Descending);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> ThenBy<TValue>(ISortableProperty<TEntity, TValue> property) where TValue : IComparable
        {
            SortBy(property, SortDirection.Ascending);
            return this;
        }

        /// <inheritdoc />
        public IODataOrderedQuery<TEntity> ThenByDescending<TValue>(ISortableProperty<TEntity, TValue> property) where TValue : IComparable
        {
            SortBy(property, SortDirection.Descending);
            return this;
        }

        private void SortBy(ISortableProperty<TEntity> property, SortDirection direction)
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
        public IODataQuery<TEntity> Offset(int? count)
        {
            _offset = count;
            return this;
        }

        /// <inheritdoc />
        public IODataQuery<TEntity> Limit(int? count)
        {
            _limit = count;
            return this;
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IEntity<TEntity>> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        )
        {
            var request = CreateFindRequest();

            var currentCount = 0;
            
            IFindResponse<TEntity>? currentResponse = await _oDataClient.FindAsync(_entityType, request, cancellationToken);
            while (currentResponse != null)
            {
                if (currentCount < _offset)
                {
                    continue;
                }
                
                foreach (var entity in currentResponse.Value)
                {
                    yield return entity;
                    currentCount++;

                    if (currentCount == _limit)
                    {
                        yield break;
                    }
                }

                var nextRequest = CreateFindNextRequest(currentCount);
                currentResponse = await _oDataClient.FindNextAsync(currentResponse, nextRequest, cancellationToken);
            }
        }

        private ODataFindRequest<TEntity> CreateFindRequest()
        {
            var maxPageSize = MinimumMaxPageSize(0);
            var request = new ODataFindRequest<TEntity>(_filter, _selection, _expansions, _sorting, maxPageSize);
            return request;
        }

        private ODataFindNextRequest<TEntity> CreateFindNextRequest(int currentCount)
        {
            var maxPageSize = MinimumMaxPageSize(currentCount);
            var request = new ODataFindNextRequest<TEntity>(maxPageSize);
            return request;
        }

        private int? MinimumMaxPageSize(int currentCount)
        {
            return _limit - currentCount < _maxPageSize ? _limit - currentCount : _maxPageSize;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var request = CreateFindRequest();
            var queryString = request.ToQueryString(_valueFormatter, QueryStringFormatting.None);

            var expression = (queryString == string.Empty ? "<empty>" : queryString);
            var maxPageSize = _maxPageSize?.ToString() ?? "<empty>";
            
            return $"Expression: {expression}" +
                   $"{Environment.NewLine}" +
                   $"MaxPageSize: {maxPageSize}";
        }
    }
}