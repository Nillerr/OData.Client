using System;
using System.Linq.Expressions;

namespace OData.Client
{
    public sealed class Entity<TEntity>
    {
        public bool TryGetValue<TValue>(Expression<Func<TEntity, TValue>> field, out TValue value)
        {
            throw new NotImplementedException();
        }
    }
}