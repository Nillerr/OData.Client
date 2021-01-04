namespace OData.Client
{
    public interface IRefProperty<out TEntity> : IProperty<TEntity>
        where TEntity : IEntity
    {
    }
    
    public interface IRefProperty<out TEntity, out TOther> : IRefProperty<TEntity>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}