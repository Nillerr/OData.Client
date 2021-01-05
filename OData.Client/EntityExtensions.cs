using System.Diagnostics.CodeAnalysis;

namespace OData.Client
{
    /// <summary>
    /// Extensions for working with <see cref="IRequired{TEntity,TValue}"/> and
    /// <see cref="IRequiredRef{TEntity,TOther}"/> properties on entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Tries to get the value with the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="source">The entity.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns><see langword="true"/> if a value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ODataNullValueException">The value of the property was <see langword="null"/>.</exception>
        public static bool TryGetValue<TEntity, TValue>(
            this IEntity<TEntity> source,
            IRequired<TEntity, TValue> property,
            [MaybeNullWhen(false)] out TValue value
        )
            where TEntity : IEntity
            where TValue : notnull
        {
            var optional = new Optional<TEntity, TValue>(property.SelectableName);
            if (source.TryGetValue(optional, out var nullable))
            {
                value = CheckNotNull(nullable, property);
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Gets the value with the specified <paramref name="property"/> converted to the specified type.
        /// </summary>
        /// <param name="source">The entity.</param>
        /// <param name="property">The property.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        /// <exception cref="ODataNullValueException">The value of the property was <see langword="null"/>.</exception>
        public static TValue Value<TEntity, TValue>(this IEntity<TEntity> source, IRequired<TEntity, TValue> property)
            where TEntity : IEntity
            where TValue : notnull
        {
            var optional = new Optional<TEntity, TValue>(property.SelectableName);
            var nullable = source.Value(optional);
            return CheckNotNull(nullable, property);
        }

        /// <summary>
        /// Tries to get the entity referenced by the specified single-valued navigation <paramref name="property"/>. 
        /// </summary>
        /// <param name="source">The entity.</param>
        /// <param name="property">The single-valued navigation property.</param>
        /// <param name="other">The referenced entity type.</param>
        /// <param name="entity">The referenced entity, or <see langword="null"/> if no entity is referenced.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TOther">The type of referenced entity.</typeparam>
        /// <returns><see langword="true"/> if an entity was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ODataNullValueException">The value of the property was <see langword="null"/>.</exception>
        public static bool TryGetEntity<TEntity, TOther>(
            this IEntity<TEntity> source,
            IRequiredRef<TEntity, TOther> property,
            IEntityType<TOther> other,
            [MaybeNullWhen(false)] out IEntity<TOther> entity
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var optional = new OptionalRef<TEntity, TOther>(property.SelectableName);
            if (source.TryGetEntity(optional, other, out var nullable))
            {
                entity = CheckNotNull(nullable, property);
                return true;
            }

            entity = default;
            return false;
        }

        /// <summary>
        /// Gets the entity referenced by the specified single-valued navigation <paramref name="property"/>.
        /// </summary>
        /// <param name="source">The entity.</param>
        /// <param name="property">The single-valued navigation property.</param>
        /// <param name="other">The referenced entity type.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TOther">The type of referenced entity.</typeparam>
        /// <returns>The referenced entity.</returns>
        /// <exception cref="ODataNullValueException">The value of the property was <see langword="null"/>.</exception>
        public static IEntity<TOther> Entity<TEntity, TOther>(
            this IEntity<TEntity> source,
            IRequiredRef<TEntity, TOther> property,
            IEntityType<TOther> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var optional = new OptionalRef<TEntity, TOther>(property.SelectableName);
            var nullable = source.Entity(optional, other);
            return CheckNotNull(nullable, property);
        }

        /// <summary>
        /// Tries to get the id of the referenced entity with the specified navigation <paramref name="property"/>.
        /// </summary>
        /// <param name="source">The entity.</param>
        /// <param name="property">The navigation property.</param>
        /// <param name="other">The name of the referenced entity.</param>
        /// <param name="id">The referenced entity id.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TOther">The referenced entity type.</typeparam>
        /// <returns><see langword="true"/> if a value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ODataNullValueException">The value of the property was <see langword="null"/>.</exception>
        public static bool TryGetReference<TEntity, TOther>(
            this IEntity<TEntity> source,
            IRequiredRef<TEntity, TOther> property,
            IEntityType<TOther> other,
            [MaybeNullWhen(false)] out IEntityId<TOther> id
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var optional = new OptionalRef<TEntity, TOther>(property.SelectableName);
            if (source.TryGetReference(optional, other, out var nullable))
            {
                id = CheckNotNull(nullable, property);
                return true;
            }

            id = default;
            return false;
        }

        /// <summary>
        /// Gets the id of the entity using the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property">The property.</param>
        /// <param name="other">The name of the referenced entity</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TOther">The referenced entity type.</typeparam>
        /// <returns>The converted value.</returns>
        /// <exception cref="ODataNullValueException">The value of the property was <see langword="null"/>.</exception>
        public static IEntityId<TOther> Reference<TEntity, TOther>(
            this IEntity<TEntity> source,
            IRequiredRef<TEntity, TOther> property,
            IEntityType<TOther> other
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            var optional = new OptionalRef<TEntity, TOther>(property.SelectableName);
            var nullable = source.Reference(optional, other);
            return CheckNotNull(nullable, property);
        }

        /// <summary>
        /// Checks if <paramref name="value"/> is <see langword="null"/>, and if so, throws an exception.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="property">The property containing the value.</param>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The value.</returns>
        /// <exception cref="ODataNullValueException"><paramref name="value"/> was <see langword="null"/>.</exception>
        private static TValue CheckNotNull<TValue>(TValue? value, IProperty property)
            where TValue : notnull
        {
            if (value == null)
            {
                throw new ODataNullValueException($"The value in the entity for required property '{property.SelectableName}' was null.", property);
            }

            return value;
        }

        /// <summary>
        /// Checks if <paramref name="value"/> is <see langword="null"/>, and if so, throws an exception.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="property">The property containing the value.</param>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The value.</returns>
        /// <exception cref="ODataNullValueException"><paramref name="value"/> was <see langword="null"/>.</exception>
        private static TValue CheckNotNull<TValue>(TValue? value, IRefProperty property)
            where TValue : notnull
        {
            if (value == null)
            {
                throw new ODataNullRefException($"The value in the entity for required property '{property.SelectableName}' was null.", property);
            }

            return value;
        }
    }
}