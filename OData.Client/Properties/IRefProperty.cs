using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A navigation property of an entity.
    /// </summary>
    public interface IRefProperty : ISelectableProperty, IExpandableProperty
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        string Name { get; }
    }
    
    /// <summary>
    /// A navigation property of a specific entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IRefProperty<[UsedImplicitly] out TEntity> : IRefProperty, ISelectableProperty<TEntity>, IExpandableProperty<TEntity>
        where TEntity : IEntity
    {
    }
    
    /// <summary>
    /// A navigation property of a specific entity, with a specific referenced entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of referenced entity.</typeparam>
    public interface IRefProperty<out TEntity, [UsedImplicitly] out TOther> : IRefProperty<TEntity>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}