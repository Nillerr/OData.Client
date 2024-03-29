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
        private readonly IExpressionFormatter _expressionFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataQuery{TEntity}"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="oDataClient">The OData client.</param>
        /// <param name="expressionFormatter">The value formatter.</param>
        public ODataQuery(IEntityType<TEntity> entityType, IODataClient oDataClient, IExpressionFormatter expressionFormatter)
        {
            _entityType = entityType;
            _oDataClient = oDataClient;
            _expressionFormatter = expressionFormatter;
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
            if (typeof(TOther) == typeof(IEntity))
            {
                throw new InvalidOperationException(
                    "Cannot expand an unspecified reference property. When expanding an unspecified reference " +
                    $"property of type {nameof(IEntity)}, the type to expand must be specified. Use the " +
                    "Expand(this IODataQuery<TEntity>, IRef<TEntity, IEntity>, IEntityType<TOther>) " +
                    "extension method to specify the type to expand.");
            }
            
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
        public IODataFindRequest<TEntity> ToFindRequest()
        {
            var maxPageSize = MinimumMaxPageSize(0);
            var request = new ODataFindRequest<TEntity>(_filter, _selection, _expansions, _sorting, maxPageSize);
            return request;
        }

        /// <inheritdoc />
        public IODataFindRequestHeaders<TEntity> ToFindNextRequest(int currentCount)
        {
            var maxPageSize = MinimumMaxPageSize(currentCount);
            var request = new ODataFindNextRequest<TEntity>(maxPageSize);
            return request;
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IEntity<TEntity>> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        )
        {
            var request = ToFindRequest();

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

                var nextRequest = ToFindNextRequest(currentCount);
                currentResponse = await _oDataClient.FindNextAsync(currentResponse, nextRequest, cancellationToken);
            }
        }

        private int? MinimumMaxPageSize(int currentCount)
        {
            return _limit - currentCount < _maxPageSize ? _limit - currentCount : _maxPageSize;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var request = ToFindRequest();
            var queryString = request.ToQueryString(_expressionFormatter, QueryStringFormatting.None);

            var expression = (queryString == string.Empty ? "<empty>" : queryString);
            var maxPageSize = _maxPageSize?.ToString() ?? "<empty>";
            
            return $"Expression: {expression}{Environment.NewLine}" +
                   $"MaxPageSize: {maxPageSize}";
        }
    }
}