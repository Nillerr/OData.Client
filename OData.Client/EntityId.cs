using System;

namespace OData.Client
{
    public sealed class EntityId<TEntity> : IEntityId<TEntity>, IEquatable<EntityId<TEntity>> 
        where TEntity : IEntity
    {
        public EntityId(Guid id, IEntityName<TEntity> name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        
        public IEntityName<TEntity> Name { get; }

        public static implicit operator Guid(EntityId<TEntity> source) => source.Id;

        public bool Equals(EntityId<TEntity>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && Name.Equals(other.Name);
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is EntityId<TEntity> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Id, Name);

        public static bool operator ==(EntityId<TEntity>? left, EntityId<TEntity>? right) => Equals(left, right);
        public static bool operator !=(EntityId<TEntity>? left, EntityId<TEntity>? right) => !Equals(left, right);

        public string ToString(string? format, IFormatProvider? formatProvider) => Id.ToString("D");

        public override string ToString() => $"/{Name}({Id:D})";
    }
}