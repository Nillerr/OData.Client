using System;

namespace OData.Client
{
    public interface IEntityId : IFormattable
    {
        Guid Id { get; }
    }

    public interface IEntityId<out TEntity> : IEntityId where TEntity : IEntity
    {
        IEntityName<TEntity> Name { get; }
    }
}