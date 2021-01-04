using System;

namespace OData.Client
{
    public static class EntityNameExtensions
    {
        public static EntityId<TEntity> Id<TEntity>(this IEntityType<TEntity> type, Guid id) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(id, type);
        }
        
        public static EntityId<TEntity> ParseId<TEntity>(this IEntityType<TEntity> type, string input) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(Guid.Parse(input), type);
        }
    }
}