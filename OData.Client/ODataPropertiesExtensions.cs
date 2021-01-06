using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace OData.Client
{
    /// <summary>
    /// A set of extensions for <see cref="IODataProperties{TEntity}"/>.
    /// </summary>
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
            IRefs<TEntity, TOther> property,
            params IEntityId<TOther>[] ids
        )
            where TEntity : IEntity
            where TOther : IEntity
        {
            return properties.BindAll(property, ids.AsEnumerable());
        }

        internal static async Task<HttpContent> ToHttpContentAsync<TEntity>(this IODataProperties<TEntity> properties)
            where TEntity : IEntity
        {
            await using var propertiesStream = new MemoryStream();
            await properties.WriteToAsync(propertiesStream);

            var propertiesBuffer = propertiesStream.GetBuffer();
            var contentStream = new MemoryStream(propertiesBuffer, false);
            
            var content = new StreamContent(contentStream);
            
            var mediaType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            mediaType.CharSet = Encoding.UTF8.WebName;
            
            content.Headers.ContentType = mediaType;
            
            return content;
        }
    }
}