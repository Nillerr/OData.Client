namespace OData.Client
{
    /// <summary>
    /// A single-valued navigation property of a specific entity, with a specific referenced entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of referenced entity.</typeparam>
    public interface IRef<out TEntity, out TOther> : IRefProperty<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
        IProperty<TEntity, IEntityId<TOther>> Value();
    }
}