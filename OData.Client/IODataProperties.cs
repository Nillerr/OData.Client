using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace OData.Client
{
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
        /// Binds an <paramref name="id"/> to a <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property to bind to.</param>
        /// <param name="id">The id to bind to the property.</param>
        /// <typeparam name="TOther">The type of entity to bind to the property.</typeparam>
        /// <returns>This instance, for fluent chaining.</returns>
        IODataProperties<TEntity> Bind<TOther>(IProperty<TEntity, TOther> property, IEntityId<TOther> id)
            where TOther : IEntity;

        /// <summary>
        /// Binds multiple <paramref name="ids"/> to a <paramref name="property"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="ids"></param>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        IODataProperties<TEntity> BindAll<TOther>(
            IProperty<TEntity, IEnumerable<TOther>> property,
            IEnumerable<IEntityId<TOther>> ids
        )
            where TOther : IEntity;

        void WriteTo(Stream stream);
    }
}