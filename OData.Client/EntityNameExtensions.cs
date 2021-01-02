using System;

namespace OData.Client
{
    public static class EntityNameExtensions
    {
        public static IEntityId<TEntity> Id<TEntity>(this EntityName<TEntity> name, Guid id) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(id, name);
        }
        
        public static IEntityId<TEntity> ParseId<TEntity>(this EntityName<TEntity> name, string input) 
            where TEntity : IEntity
        {
            return new EntityId<TEntity>(Guid.Parse(input), name);
        }
    }
}