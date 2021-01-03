using System;

namespace OData.Client
{
    internal sealed class EntityId<TEntity> : IEntityId<TEntity> where TEntity : IEntity
    {
        public EntityId(Guid id, IEntityName<TEntity> name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        
        public IEntityName<TEntity> Name { get; }

        public static implicit operator Guid(EntityId<TEntity> source) => source.Id;

        public string ToString(string? format, IFormatProvider? formatProvider) => Id.ToString("D");
    }
}