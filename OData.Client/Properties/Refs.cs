using System;

namespace OData.Client
{
    /// <inheritdoc cref="IRefs{TEntity,TOther}" />
    public sealed class Refs<TEntity, TOther> :
        IRefs<TEntity, TOther>,
        IEquatable<Refs<TEntity, TOther>>
        where TEntity : IEntity
        where TOther : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Refs{TEntity,TOther}"/> class.
        /// </summary>
        /// <param name="prefix">The property prefix.</param>
        /// <param name="name">The property name.</param>
        public Refs(string prefix, string name)
        {
            Name = prefix + name;
            SelectableName = $"{prefix}_{name}_value";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Refs{TEntity,TOther}"/> class.
        /// </summary>
        /// <param name="name">The property name.</param>
        public Refs(string name)
        {
            Name = name;
            SelectableName = $"_{name}_value";
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SelectableName { get; }

        /// <inheritdoc />
        public string ExpandableName => Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Refs{TEntity,TOther}"/> class using the string as the property
        /// name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The newly created <see cref="Refs{TEntity,TOther}"/> instance.</returns>
        public static implicit operator Refs<TEntity, TOther>(string name) => new(name);

        /// <inheritdoc />
        public bool Equals(Refs<TEntity, TOther>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && SelectableName == other.SelectableName;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Refs<TEntity, TOther> optional && Equals(optional);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => SelectableName.GetHashCode();
        
        /// <summary>
        /// Determines whether the left object is equal to the right object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Refs<TEntity, TOther>? left, Refs<TEntity, TOther>? right) => Equals(left, right);
        
        /// <summary>
        /// Determines whether the left object is not equal to the right object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns><see langword="true"/> if the two objects are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Refs<TEntity, TOther>? left, Refs<TEntity, TOther>? right) => !Equals(left, right);

        /// <inheritdoc />
        public override string ToString() => $"{nameof(Name)}: {Name}, {nameof(SelectableName)}: {SelectableName}";
    }
}