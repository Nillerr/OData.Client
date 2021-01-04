using System;
using System.Collections.Generic;

namespace OData.Client
{
    public sealed class FindResponse<TEntity> : IFindResponse<TEntity> where TEntity : IEntity
    {
        public FindResponse(
            Uri context,
            Uri? nextLink,
            IReadOnlyList<IEntity<TEntity>> value,
            IODataFindRequest<TEntity> request
        )
        {
            Context = context;
            NextLink = nextLink;
            Value = value;
            Request = request;
        }

        public Uri Context { get; }
        public Uri? NextLink { get; }
        public IReadOnlyList<IEntity<TEntity>> Value { get; }
        public IODataFindRequest<TEntity> Request { get; }
    }
}