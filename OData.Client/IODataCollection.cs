namespace OData.Client
{
    public interface IODataCollection<TEntity> where TEntity : IEntity
    {
        IODataQuery<TEntity> Find(ODataFilter<TEntity> filter);

        // Task<TEntity?> RetrieveAsync(Guid id);
        // Task<TProjection?> RetrieveAsync<TProjection>(Guid id, Expression<Func<TEntity, TProjection>> selection);

        // Task<Guid> InsertAsync(TEntity entity);
        // Task<Guid> UpdateAsync(TEntity entity);
        // Task DeleteAsync(Guid id);
        
        // Task AssociatedAsync<TValue, TOther>(Guid id, Field<TEntity, TValue> field, IODataCollection<TOther> other, Guid otherId);
        // Task DisassociateAsync<TValue, TOther>(Guid id, Field<TEntity, TValue> field, IODataCollection<TOther> other, Guid otherId);
    }
}