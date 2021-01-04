namespace OData.Client
{
    public interface IRefProperty<out TEntity, out TOther> : IProperty<TEntity>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}