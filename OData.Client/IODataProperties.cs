using System.Collections.Generic;
using System.IO;

namespace OData.Client
{
    /// <summary>
    /// A specification of properties to set when creating or updating an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public interface IODataProperties<in TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Sets the value of <paramref name="property"/> to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="property">The property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <typeparam name="TValue">The type of value of the property.</typeparam>
        /// <returns>This instance, for fluent chaining.</returns>
        IODataProperties<TEntity> Set<TValue>(IProperty<TEntity, TValue> property, TValue value);

        /// <summary>
        /// Binds an <paramref name="id"/> to a single-valued navigation <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property to bind to.</param>
        /// <param name="id">The id to bind to the property.</param>
        /// <typeparam name="TOther">The type of entity to bind to the property.</typeparam>
        /// <returns>This instance, for fluent chaining.</returns>
        IODataProperties<TEntity> Bind<TOther>(IRef<TEntity, TOther> property, IEntityId<TOther> id)
            where TOther : IEntity;

        /// <summary>
        /// Binds multiple <paramref name="ids"/> to a collection-valued navigation <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The collection-valued navigation property.</param>
        /// <param name="ids">The referenced entity ids.</param>
        /// <typeparam name="TOther">The type of referenced entities.</typeparam>
        /// <returns>This instance, for fluent chaining.</returns>
        IODataProperties<TEntity> BindAll<TOther>(
            IRefs<TEntity, TOther> property,
            IEnumerable<IEntityId<TOther>> ids
        )
            where TOther : IEntity;

        /// <summary>
        /// Writes the properties as JSON to the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream</param>
        void WriteTo(Stream stream);
    }
}