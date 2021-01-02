using System;
using System.Collections.Generic;

namespace OData.Client
{
    public interface IFindResponse<TEntity>
        where TEntity : IEntity
    {
        Uri Context { get; }
        Uri? NextLink { get; }
        IReadOnlyList<IEntity<TEntity>> Value { get; }
    }
}