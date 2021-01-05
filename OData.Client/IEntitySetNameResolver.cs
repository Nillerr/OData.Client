namespace OData.Client
{
    public interface IEntitySetNameResolver
    {
        string EntitySetName<TEntity>(IEntityType<TEntity> type)
            where TEntity : IEntity;
    }
}