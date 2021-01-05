using JetBrains.Annotations;

namespace OData.Client
{
    public interface IExpandableProperty
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        string ExpandableName { get; }
    }
    
    public interface IExpandableProperty<[UsedImplicitly] out TEntity> : IExpandableProperty
        where TEntity : IEntity
    {
    }
}