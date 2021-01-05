using System.Collections.Generic;

namespace OData.Client
{
    public interface IODataFindRequestHeaders<TEntity>
        where TEntity : IEntity
    {
        int? MaxPageSize { get; }
    }
    public sealed class ODataFindNextRequest<TEntity> : IODataFindRequestHeaders<TEntity>
        where TEntity : IEntity
    {
        public ODataFindNextRequest(int? maxPageSize)
        {
            MaxPageSize = maxPageSize;
        }

        public int? MaxPageSize { get; }
    }
    
    /// <inheritdoc />
    public sealed class ODataFindRequest<TEntity> : IODataFindRequest<TEntity>, IODataFindRequestHeaders<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataFindRequest{TEntity}"/> class.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="selection">The selection to apply.</param>
        /// <param name="expansions">The expansions to apply.</param>
        /// <param name="sorting">The sorting to apply.</param>
        /// <param name="maxPageSize">The maximum number of results per page.</param>
        public ODataFindRequest(
            ODataFilter<TEntity>? filter,
            IEnumerable<IProperty<TEntity>> selection,
            IEnumerable<ODataExpansion<TEntity>> expansions,
            IEnumerable<Sorting<TEntity>> sorting,
            int? maxPageSize
        )
        {
            Filter = filter;
            Selection = selection;
            Expansions = expansions;
            MaxPageSize = maxPageSize;
            Sorting = sorting;
        }

        /// <inheritdoc />
        public ODataFilter<TEntity>? Filter { get; }

        /// <inheritdoc />
        public IEnumerable<IProperty<TEntity>> Selection { get; }

        /// <inheritdoc />
        public IEnumerable<ODataExpansion<TEntity>> Expansions { get; }

        /// <inheritdoc />
        public IEnumerable<Sorting<TEntity>> Sorting { get; }

        /// <inheritdoc />
        public int? MaxPageSize { get; }
    }
}