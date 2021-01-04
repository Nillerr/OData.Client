using System;

namespace OData.Client
{
    /// <inheritdoc cref="IEntityId{TEntity}" />
    public sealed class EntityId<TEntity> : IEntityId<TEntity>, IEquatable<EntityId<TEntity>> 
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityId{TEntity}"/> class.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="type">The type of the entity.</param>
        public EntityId(Guid id, IEntityType<TEntity> type)
        {
            Id = id;
            Type = type;
        }

        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public IEntityType<TEntity> Type { get; }

        /// <inheritdoc />
        public bool Equals(EntityId<TEntity>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && Type.Equals(other.Type);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is EntityId<TEntity> other && Equals(other);
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => HashCode.Combine(Id, Type);

        /// <summary>
        /// Determines whether the left object is equal to the right object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(EntityId<TEntity>? left, EntityId<TEntity>? right) => Equals(left, right);
        
        /// <summary>
        /// Determines whether the left object is not equal to the right object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns><see langword="true"/> if the two objects are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(EntityId<TEntity>? left, EntityId<TEntity>? right) => !Equals(left, right);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider) => Id.ToString("D");

        /// <inheritdoc />
        public override string ToString() => $"/{Type}({Id:D})";
    }
}