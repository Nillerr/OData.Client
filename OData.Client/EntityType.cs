using JetBrains.Annotations;

namespace OData.Client
{
    /// <inheritdoc />
    /// <example>
    /// <code>
    /// public sealed class Account : IEntity {
    ///     public static readonly EntityType&lt;Account&gt; EntityType = "accounts";
    /// }
    /// </code>
    /// </example>
    [PublicAPI]
    public sealed class EntityType<TEntity> : IEntityType<TEntity>
        where TEntity : IEntity
    {
        /// <inheritdoc />
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType{TEntity}"/> class.
        /// </summary>
        /// <param name="name">The pluralized name / endpoint of the entity, e.g. <c>"accounts"</c>, <c>"contacts"</c>.</param>
        /// <seealso cref="op_Implicit"/>
        public EntityType(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType{TEntity}"/> class using the string as the
        /// <see cref="Name"/>.
        /// </summary>
        /// <param name="name">The pluralized name / endpoint of the entity, e.g. <c>"accounts"</c>, <c>"contacts"</c>.</param>
        /// <returns>The new instance.</returns>
        public static implicit operator EntityType<TEntity>(string name) => new(name);
    }
}