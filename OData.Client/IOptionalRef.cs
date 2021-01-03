namespace OData.Client
{
    /// <summary>
    /// An optional property is any property that can be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of value.</typeparam>
    public interface IOptionalRef<out TEntity, out TOther> : IRef<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
    }
}