using System;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataCollection<TEntity> where TEntity : IEntity
    {
        IODataQuery<TEntity> Find();

        // Task<TEntity?> RetrieveAsync(Guid id);
        // Task<TProjection?> RetrieveAsync<TProjection>(Guid id, Expression<Func<TEntity, TProjection>> selection);

        Task<Guid> CreateAsync(Action<IODataProperties<TEntity>> set, CancellationToken cancellationToken = default);
        // Task<Guid> UpdateAsync(TEntity entity);
        Task DeleteAsync(IEntityId<TEntity> id, CancellationToken cancellationToken = default);
        
        // Task AssociatedAsync<TValue, TOther>(Guid id, Field<TEntity, TValue> field, IODataCollection<TOther> other, Guid otherId);
        // Task DisassociateAsync<TValue, TOther>(Guid id, Field<TEntity, TValue> field, IODataCollection<TOther> other, Guid otherId);
    }
}