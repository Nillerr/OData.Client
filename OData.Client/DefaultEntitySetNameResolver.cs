namespace OData.Client
{
    public sealed class DefaultEntitySetNameResolver : IEntitySetNameResolver
    {
        public string EntitySetName<TEntity>(IEntityType<TEntity> type)
            where TEntity : IEntity
        {
            if (type.Name.EndsWith("s"))
            {
                return $"{type.Name}es";
            }
            
            return $"{type.Name}s";
        }
    }
}