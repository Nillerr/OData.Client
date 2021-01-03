using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace OData.Client
{
    public static class ODataPropertiesExtensions
    {
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

        public static HttpContent ToHttpContent<TEntity>(this IODataProperties<TEntity> properties) where TEntity : IEntity
        {
            var contentStream = new MemoryStream();
            properties.WriteTo(contentStream);

            contentStream.Seek(0, SeekOrigin.Begin);

            var content = new StreamContent(contentStream);
            return content;
        }

        public static string SelectableName<TEntity>(this IProperty<TEntity> property) where TEntity : IEntity
        {
            if (property.ValueType.IsAssignableTo(typeof(IEntity)))
            {
                return $"_{property.Name}_value";
            }

            if (property.ValueType.IsEnumerableType(out var valueType))
            {
                return property.Name;
            }

            return property.Name;
        }
    }
}