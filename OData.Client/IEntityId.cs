using System;

namespace OData.Client
{
    public interface IEntityId<out TEntity> : IFormattable where TEntity : IEntity
    {
        Guid Id { get; }
        
        IEntityName<TEntity> Name { get; }
    }
}