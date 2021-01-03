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

    /// <summary>
    /// A required property is any property that cannot be <see langword="null"/>, including collection-valued
    /// navigation properties.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IRequired<out TEntity, out TValue> : IProperty<TEntity, TValue>
        where TEntity : IEntity
        where TValue : notnull
    {
        IOptional<TEntity, TValue> AsOptional();
    }

    /// <summary>
    /// An optional property is any property that can be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IOptional<out TEntity, out TValue> : IProperty<TEntity, TValue>
        where TEntity : IEntity
        where TValue : notnull
    {
    }

    public interface IRefProperty<out TEntity, out TOther> : IProperty<TEntity>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }

    public interface IRef<out TEntity, out TOther> : IRefProperty<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }

    public interface IRefs<out TEntity, out TOther> : IRefProperty<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }

    /// <summary>
    /// A required property is any property that cannot be <see langword="null"/>, including collection-valued
    /// navigation properties.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of value.</typeparam>
    public interface IRequiredRef<out TEntity, out TOther> : IRef<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
        IOptionalRef<TEntity, TOther> AsOptional();
    }

    /// <summary>
    /// An optional property is any property that can be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of value.</typeparam>
    public interface IOptionalRef<out TEntity, out TOther> : IRef<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}