using System;

namespace OData.Client
{
    public interface IProperty
    {
        string Name { get; }
        
        Type ValueType { get; }
    }
    
    public interface IProperty<out TEntity> : IProperty
        where TEntity : IEntity
    {
    }
    
    public interface IProperty<out TEntity, out TValue> : IProperty<TEntity>
        where TEntity : IEntity
    {
    }
}