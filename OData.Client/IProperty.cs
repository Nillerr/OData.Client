using System;

namespace OData.Client
{
    public interface IProperty<out TEntity>
        where TEntity : IEntity
    {
        string Name { get; }
        
        Type ValueType { get; }
    }
    
    public interface IProperty<out TEntity, out TValue> : IProperty<TEntity>
        where TEntity : IEntity
    {
    }
}