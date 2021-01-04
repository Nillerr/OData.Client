namespace OData.Client
{
    public interface IRef<out TEntity, out TOther> : IRefProperty<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}