using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task<IEntity<TEntity>> ReadEntityAsync<TEntity>(
            this HttpContent content,
            IEntitySerializer entitySerializer,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            await using var stream = await content.ReadAsStreamAsync(cancellationToken);

            var entity = await entitySerializer.DeserializeEntityAsync<TEntity>(stream);
            return entity;
        }
    }
}