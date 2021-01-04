namespace OData.Client
{
    public interface IODataSelection<in TEntity> where TEntity : IEntity
    {
        IODataSelection<TEntity> Select(IProperty<TEntity> property);
        IODataSelection<TEntity> Select(params IProperty<TEntity>[] properties);

        IODataSelection<TEntity> Expand<TOther>(IRef<TEntity, TOther> property) where TOther : IEntity;
    }
}