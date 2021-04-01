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
    [Equals]
    public sealed class EntityType<TEntity> : IEntityType<TEntity>
        where TEntity : IEntity
    {
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string IdPropertyName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType{TEntity}"/> class.
        /// </summary>
        /// <param name="name">The pluralized name / endpoint of the entity, e.g. <c>"accounts"</c>, <c>"contacts"</c>.</param>
        /// <seealso cref="op_Implicit"/>
        public EntityType(string name)
        {
            Name = name;
            IdPropertyName = $"{name}id";
        }

        private EntityType(string name, string idPropertyName)
        {
            Name = name;
            IdPropertyName = idPropertyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType{TEntity}"/> class using the string as the
        /// <see cref="Name"/>.
        /// </summary>
        /// <param name="name">The name of the entity, e.g. <c>"account"</c>, <c>"contact"</c>.</param>
        /// <returns>The new instance.</returns>
        public static implicit operator EntityType<TEntity>(string name) => new(name);

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityType{TEntity}"/> class as a child of another entity,
        /// using the specified name.
        /// </summary>
        /// <param name="name">The name of the child entity, e.g. <c>"account"</c>, <c>"contact"</c>.</param>
        /// <returns>The child entity type instance.</returns>
        public EntityType<TEntity> Child(string name) => new(name, IdPropertyName);
    }
}