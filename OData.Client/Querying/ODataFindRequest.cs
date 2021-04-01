using System;
using System.Collections.Generic;
using System.Linq;

namespace OData.Client
{
    /// <inheritdoc cref="IODataFindRequest{TEntity}" />
    public sealed class ODataFindRequest<TEntity> : IODataFindRequest<TEntity>, IEquatable<ODataFindRequest<TEntity>>
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
            IReadOnlyCollection<ISelectableProperty<TEntity>> selection,
            IReadOnlyCollection<ODataExpansion<TEntity>> expansions,
            IReadOnlyList<Sorting<TEntity>> sorting,
            int? maxPageSize)
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
        public IReadOnlyCollection<ISelectableProperty<TEntity>> Selection { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<ODataExpansion<TEntity>> Expansions { get; }

        /// <inheritdoc />
        public IReadOnlyList<Sorting<TEntity>> Sorting { get; }

        /// <inheritdoc />
        public int? MaxPageSize { get; }

        /// <inheritdoc />
        public bool Equals(ODataFindRequest<TEntity>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Nullable.Equals(Filter, other.Filter) &&
                   MaxPageSize == other.MaxPageSize &&
                   Selection.Count == other.Selection.Count && Selection.All(other.Selection.Contains) &&
                   Expansions.Count == other.Expansions.Count && Expansions.All(other.Expansions.Contains) &&
                   Sorting.Count == other.Sorting.Count && Sorting.SequenceEqual(other.Sorting);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is ODataFindRequest<TEntity> other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Filter);
            hashCode.Add(MaxPageSize);
            
            foreach (var selection in Selection)
            {
                hashCode.Add(selection);
            }

            foreach (var expansion in Expansions)
            {
                hashCode.Add(expansion);
            }

            foreach (var sorting in Sorting)
            {
                hashCode.Add(sorting);
            }

            return hashCode.ToHashCode();
        }

        public static bool operator ==(ODataFindRequest<TEntity>? left, ODataFindRequest<TEntity>? right) => Equals(left, right);
        public static bool operator !=(ODataFindRequest<TEntity>? left, ODataFindRequest<TEntity>? right) => !Equals(left, right);

        public override string ToString()
        {
            return $"{nameof(Filter)}: {Filter}, {nameof(Selection)}: {Selection}, {nameof(Expansions)}: {Expansions}, {nameof(Sorting)}: {Sorting}, {nameof(MaxPageSize)}: {MaxPageSize}";
        }
    }
}