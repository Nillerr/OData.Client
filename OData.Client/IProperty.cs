using System;

namespace OData.Client
{
    public interface IProperty<TEntity>
        where TEntity : IEntity
    {
        string Name { get; }
        
        Type ValueType { get; }
    }
}