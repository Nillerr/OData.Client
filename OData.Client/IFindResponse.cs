using System;
using System.Collections.Generic;

namespace OData.Client
{
    public interface IFindResponse<TEntity>
    {
        string Context { get; }
        List<TEntity> Value { get; }
        Uri? NextLink { get; }
    }
}