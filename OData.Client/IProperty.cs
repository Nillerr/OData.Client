using System;

namespace OData.Client
{
    /// <summary>
    /// A property of an entity.
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
    
    public interface IProperty<out TEntity> : IProperty
        where TEntity : IEntity
    {
    }

    public interface IProperty<out TEntity, out TValue> : IProperty<TEntity>
        where TEntity : IEntity
    {
    }
}