using JetBrains.Annotations;

namespace OData.Client
{
    public interface IODataFindRequestHeaders<[UsedImplicitly] out TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The maximum number of results per page.
        /// </summary>
        int? MaxPageSize { get; }
    }
}