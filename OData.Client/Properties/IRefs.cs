using JetBrains.Annotations;

namespace OData.Client
{
    public interface IRefs : IRefProperty
    {
    }
    
    public interface IRefs<[UsedImplicitly] out TEntity> : IRefs, IRefProperty<TEntity>
        where TEntity : IEntity
    {
        
    }
    
    /// <summary>
    /// A collection-valued navigation property of a specific entity, with a specific referenced entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of referenced entity.</typeparam>
    public interface IRefs<out TEntity, out TOther> : IRefProperty<TEntity, TOther>, IRefs<TEntity>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}