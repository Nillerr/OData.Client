using System;
using System.Linq;
using System.Net.Http.Headers;

namespace OData.Client
{
    internal static class HttpResponseHeaderExtensions
    {
        /// <summary>
        /// Extracts the <see cref="Guid"/> from the <c>OData-EntityId</c> header value.
        /// </summary>
        /// <remarks>
        /// The <c>OData-EntityId</c> header value has the format
        /// <c>OData-EntityId: [Organization URI]/api/data/v9.0/contacts(6124b4e0-9299-483d-9cf2-5886533215e7)</c>, and
        /// as such the <see cref="Guid"/> stored within must be extracted from the rest of the "id", as the remaining
        /// part is of little value.
        /// 
        /// TODO @nije: Revise the wording and consider introducing an EntityId type holding the type of entity as well as the Guid
        /// </remarks>
        /// <param name="headers">The HTTP response</param>
        /// <param name="name">The entity name</param>
        /// <returns>The id portion of the <c>OData-EntityId</c> header value.</returns>
        public static IEntityId<TEntity> EntityId<TEntity>(this HttpResponseHeaders headers, IEntityName<TEntity> name)
            where TEntity : IEntity
        {
            var entityIdValues = headers.GetValues("OData-EntityId");
            var entityIdValue = entityIdValues.Single();

            const int guidLength = 36;
            var entityIdString = entityIdValue.Substring(entityIdValue.Length - guidLength - 1, guidLength);

            var id = Guid.Parse(entityIdString);
            var entityId = new EntityId<TEntity>(id, name);
            return entityId;
        }
    }
}