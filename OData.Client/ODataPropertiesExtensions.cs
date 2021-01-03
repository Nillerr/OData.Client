using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace OData.Client
{
    public static class ODataPropertiesExtensions
    {
        /// <summary>
        /// Binds multiple <paramref name="ids"/> to a <paramref name="property"/>.
        /// </summary>
        /// <param name="properties">The properties specification.</param>
        /// <param name="property">The property.</param>
        /// <param name="ids">The ids.</param>
        /// <typeparam name="TOther"></typeparam>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The properties specification instance, for fluent chaining.</returns>
        public static IODataProperties<TEntity> BindAll<TEntity, TOther>(
            this IODataProperties<TEntity> properties,
            IProperty<TEntity, IEnumerable<TOther>> property,
            params IEntityId<TOther>[] ids
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return properties.BindAll(property, ids.AsEnumerable());
        }

        internal static HttpContent ToHttpContent<TEntity>(this IODataProperties<TEntity> properties) where TEntity : IEntity
        {
            var contentStream = new MemoryStream();
            properties.WriteTo(contentStream);

            contentStream.Seek(0, SeekOrigin.Begin);

            var content = new StreamContent(contentStream);
            return content;
        }
        
        /// <summary>
        /// Returns the name of the property as it is used in <c>$filter=</c> and <c>$select=</c> expressions, meaning
        /// if the property is a single-value navigation property, it will be reformatted as: <c>"_{property.Name}_value"</c>. 
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The usable name of the property.</returns>
        internal static string SelectableName(this IProperty property)
        {
            if (property.ValueType.IsAssignableTo(typeof(IEntity)))
            {
                return $"_{property.Name}_value";
            }

            // if (property.ValueType.IsEnumerableType(out var valueType))
            // {
            //     return property.Name;
            // }

            return property.Name;
        }

        /// <summary>
        /// Returns the name of the single-value navigation property as is is used in <c>$select=</c> expressions,
        /// meaning it will be reformatted as: <c>"_{property.Name}_value"</c>.
        /// </summary>
        /// <param name="property">The single-value navigation property.</param>
        /// <typeparam name="TEntity">The entity.</typeparam>
        /// <typeparam name="TOther">The entity referenced by the property.</typeparam>
        /// <returns>The value-name of the property.</returns>
        public static string ValueName<TEntity, TOther>(this IEntityReference<TEntity, TOther> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            return $"_{property.Name}_value";
        }
    }
}