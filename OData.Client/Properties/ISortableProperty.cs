using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A sortable property of an entity.
    /// </summary>
    public interface ISortableProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        string SortableName { get; }
    }
    
    /// <summary>
    /// A sortable property of a specific entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface ISortableProperty<[UsedImplicitly] out TEntity> : ISortableProperty
        where TEntity : IEntity
    {
    }

    /// <summary>
    /// A sortable property of a specific entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface ISortableProperty<[UsedImplicitly] out TEntity, [UsedImplicitly] out TValue> : ISortableProperty<TEntity>
        where TEntity : IEntity
    {
    }
}