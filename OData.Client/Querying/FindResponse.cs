using System;
using System.Collections.Generic;

namespace OData.Client
{
    /// <summary>
    /// An implementation of the response returned by an OData entity query.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public sealed class FindResponse<TEntity> : IFindResponse<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindResponse{TEntity}"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="context">The context as it is returned by the API.</param>
        /// <param name="nextLink">The link for fetching the next page, or <see langword="null"/> if no more pages are available.</param>
        /// <param name="value">The entities matching the query.</param>
        /// <param name="request">The original request.</param>
        public FindResponse(
            IEntityType<TEntity> entityType,
            Uri context,
            Uri? nextLink,
            IReadOnlyList<IEntity<TEntity>> value,
            IODataFindRequest<TEntity> request
        )
        {
            EntityType = entityType;
            Context = context;
            NextLink = nextLink;
            Value = value;
            Request = request;
        }

        /// <inheritdoc />
        public IEntityType<TEntity> EntityType { get; }

        /// <inheritdoc />
        public Uri Context { get; }

        /// <inheritdoc />
        public Uri? NextLink { get; }

        /// <inheritdoc />
        public IReadOnlyList<IEntity<TEntity>> Value { get; }

        /// <inheritdoc />
        public IODataFindRequest<TEntity> Request { get; }
    }
}