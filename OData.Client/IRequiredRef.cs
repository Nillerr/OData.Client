namespace OData.Client
{
    /// <summary>
    /// A required property is any property that cannot be <see langword="null"/>, including collection-valued
    /// navigation properties.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOther">The type of value.</typeparam>
    public interface IRequiredRef<out TEntity, out TOther> : IRef<TEntity, TOther>
        where TEntity : IEntity
        where TOther : IEntity
    {
        IOptionalRef<TEntity, TOther> AsOptional();
    }
}