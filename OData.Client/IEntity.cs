using System.Collections.Generic;

namespace OData.Client
{
    /// <summary>
    /// Marker interface for entity type definition. While they share names, this interface is not related to
    /// <see cref="IEntity{TEntity}"/>. 
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// A collection of properties retrieved for an entity. While they share names, this interface is not related to
    /// <see cref="IEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IEntity<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Returns the id of the entity specified by <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The id property.</param>
        /// <returns>The id of the entity.</returns>
        IEntityId<TEntity> Id(IRequired<TEntity, IEntityId<TEntity>> property);
        
        /// <summary>
        /// Checks if the specified <paramref name="property"/> is present.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns><see langword="true"/> if the property is present; otherwise, <see langword="false"/>.</returns>
        bool Contains(IProperty<TEntity> property);

        /// <summary>
        /// Tries to get the value with the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns><see langword="true"/> if a value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        bool TryGetValue<TValue>(IOptional<TEntity, TValue> property, out TValue? value) where TValue : notnull;

        /// <summary>
        /// Gets the value with the specified <paramref name="property"/> converted to the specified type.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        TValue? Value<TValue>(IOptional<TEntity, TValue> property) where TValue : notnull;

        /// <summary>
        /// Tries to get the reference entity with the specified single-valued navigation <paramref name="property"/>. 
        /// </summary>
        /// <param name="property">The single-valued navigation property.</param>
        /// <param name="other">The type of the referenced entity.</param>
        /// <param name="entity">The referenced entity.</param>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        bool TryGetEntity<TOther>(
            IOptionalRef<TEntity, TOther> property,
            IEntityType<TOther> other,
            out IEntity<TOther>? entity
        )
            where TOther : IEntity;

        IEntity<TOther>? Entity<TOther>(IOptionalRef<TEntity, TOther> property, IEntityType<TOther> other)
            where TOther : IEntity;

        bool TryGetEntities<TOther>(
            IRequired<TEntity, IEnumerable<TOther>> property,
            IEntityType<TOther> other,
            out IEnumerable<IEntity<TOther>>? entities
        )
            where TOther : IEntity;

        IEnumerable<IEntity<TOther>>? Entities<TOther>(
            IRequired<TEntity, IEnumerable<TOther>> property,
            IEntityType<TOther> other
        )
            where TOther : IEntity;

        /// <summary>
        /// Checks if the specified navigation <paramref name="property"/> is present.
        /// </summary>
        /// <param name="property">The navigation property.</param>
        /// <typeparam name="TOther">The referenced entity type.</typeparam>
        /// <returns><see langword="true"/> if the navigation property is present; otherwise, <see langword="false"/>.</returns>
        bool ContainsReference<TOther>(IRef<TEntity, TOther> property)
            where TOther : IEntity;

        /// <summary>
        /// Tries to get the id of the referenced entity with the specified navigation <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The navigation property.</param>
        /// <param name="other"></param>
        /// <param name="id">The referenced entity id.</param>
        /// <typeparam name="TOther">The referenced entity type.</typeparam>
        /// <returns><see langword="true"/> if a value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        bool TryGetReference<TOther>(
            IOptionalRef<TEntity, TOther> property,
            IEntityType<TOther> other,
            out IEntityId<TOther>? id
        )
            where TOther : IEntity;

        /// <summary>
        /// Gets the id of the entity using the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The converted value.</returns>
        IEntityId<TOther>? Reference<TOther>(IOptionalRef<TEntity, TOther> property, IEntityType<TOther> other)
            where TOther : IEntity;

        /// <summary>
        /// Returns a JSON representation of the selected properties in the entity.
        /// </summary>
        /// <returns>The JSON representation of the selected properties in the entity.</returns>
        string ToJson(Formatting formatting);
    }
}