using System;

namespace OData.Client
{
    public interface IEntityId<out TEntity> : IFormattable where TEntity : IEntity
    {
        Guid Id { get; }
        
        IEntityType<TEntity> Type { get; }
    }
}