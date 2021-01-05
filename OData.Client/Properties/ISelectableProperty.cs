using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// A selectable property of an entity.
    /// </summary>
    public interface ISelectableProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        string SelectableName { get; }
    }
    
    /// <summary>
    /// A selectable property of a specific entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface ISelectableProperty<[UsedImplicitly] out TEntity> : ISelectableProperty
        where TEntity : IEntity
    {
    }
}