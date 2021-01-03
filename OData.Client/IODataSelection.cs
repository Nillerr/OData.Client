namespace OData.Client
{
    public interface IODataSelection<TEntity> where TEntity : IEntity
    {
        IODataSelection<TEntity> Select(IProperty<TEntity> property);
        IODataSelection<TEntity> Select(params IProperty<TEntity>[] properties);

        IODataSelection<TEntity> Expand<TOther>(Property<TEntity, TOther?> property) where TOther : IEntity;
    }
}