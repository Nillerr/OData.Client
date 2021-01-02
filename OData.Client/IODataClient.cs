using System;
using System.Threading.Tasks;

namespace OData.Client
{
    public interface IODataClient
    {
        // Task<TEntity?> RetrieveAsync<TEntity>(EntityName<TEntity> name, );
        //
        // Task<TEntity?> RetrieveAsync<TEntity>(EntityName<TEntity> name, Guid id);
        // Task<TEntity?> RetrieveAsync<TEntity>(EntityName<TEntity> name, Guid id, Selection<TEntity> selection);
        //
        // Task<Guid> InsertAsync<TEntity>(EntityName<TEntity> name, TEntity entity);
        // Task<Guid> UpdateAsync<TEntity>(EntityName<TEntity> name, TEntity entity);
        // Task DeleteAsync<TEntity>(EntityName<TEntity> name, Guid id);
        //
        // Task AssociatedAsync<TSource, TValue, TOther>(EntityName<TSource> source, Field<TSource, TValue> field, EntityName<TOther> other);
        // Task DisassociateAsync<TSource, TValue, TOther>(EntityName<TSource> source, Field<TSource, TValue> field, EntityName<TOther> other);
    }
}