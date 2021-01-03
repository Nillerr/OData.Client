using System;

namespace OData.Client
{
    public static class EntityNameExtensions
    {
        public static EntityId<TEntity> Id<TEntity>(this IEntityName<TEntity> name, Guid id) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(id, name);
        }
        
        public static EntityId<TEntity> ParseId<TEntity>(this IEntityName<TEntity> name, string input) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(Guid.Parse(input), name);
        }
    }
}