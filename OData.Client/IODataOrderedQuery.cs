using System;

namespace OData.Client
{
    public interface IODataOrderedQuery<TEntity> : IODataQuery<TEntity> where TEntity : IEntity
    {
        IODataOrderedQuery<TEntity> ThenBy<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable;

        IODataOrderedQuery<TEntity> ThenByDescending<TValue>(IProperty<TEntity, TValue?> property) where TValue : IComparable;
    }
}