namespace OData.Client
{
    public interface IRefs<out TEntity, out TOther> : IRefProperty<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}