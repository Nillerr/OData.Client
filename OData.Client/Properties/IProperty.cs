using System;
using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A property of any entity.
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        /// <remarks>
        /// For navigation properties, this is the name of the property itself, as it would be used in an
        /// <c>$expand=</c> expression.
        /// </remarks>
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
    public interface IProperty<[UsedImplicitly] out TEntity> : IProperty
        where TEntity : IEntity
    {
    }

    /// <summary>
    /// A property of a specific entity type, with a specific value.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IProperty<out TEntity, [UsedImplicitly] out TValue> : IProperty<TEntity>
        where TEntity : IEntity
    {
    }
}