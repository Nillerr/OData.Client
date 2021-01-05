using JetBrains.Annotations;

namespace OData.Client
{
    /// <summary>
    /// An expandable property for an entity.
    /// </summary>
    public interface IExpandableProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        string ExpandableName { get; }
    }
    
    /// <summary>
    /// An expandable property for a specific entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IExpandableProperty<[UsedImplicitly] out TEntity> : IExpandableProperty
        where TEntity : IEntity
    {
    }
}