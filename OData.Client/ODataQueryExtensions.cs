using System.Collections.Generic;
using System.Threading.Tasks;

namespace OData.Client
{
    public static class ODataQueryExtensions
    {
        public static async Task<IEntity<TEntity>[]> ToArrayAsync<TEntity>(this IODataQuery<TEntity> query)
            where TEntity : IEntity
        {
            var list = await query.ToListAsync();
            return list.ToArray();
        }
        
        public static async Task<List<IEntity<TEntity>>> ToListAsync<TEntity>(this IODataQuery<TEntity> query)
            where TEntity : IEntity
        {
            var entities = new List<IEntity<TEntity>>();
            
            await foreach (var entity in query)
            {
                entities.Add(entity);
            }

            return entities;
        }
    }
}