using System;
using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A property of any entity.
    /// </summary>
    public interface IProperty : ISelectableProperty, ISortableProperty
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// The type of value stored in this property.
        /// </summary>
        Type ValueType { get; }
        
        /// <summary>
        /// The type of entity.
        /// </summary>
        Type EntityType { get; }
    }
    
    /// <summary>
    /// A property of a specific entity type. 
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IProperty<[UsedImplicitly] out TEntity> : IProperty, ISelectableProperty<TEntity>, ISortableProperty<TEntity>
        where TEntity : IEntity
    {
    }

    /// <summary>
    /// A property of a specific entity type, with a specific value.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IProperty<out TEntity, [UsedImplicitly] out TValue> : IProperty<TEntity>, ISortableProperty<TEntity, TValue>
        where TEntity : IEntity
        where TValue : notnull
    {
    }
}