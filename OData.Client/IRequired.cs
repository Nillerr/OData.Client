namespace OData.Client
{
    /// <summary>
    /// A required property is any property that cannot be <see langword="null"/>, including collection-valued
    /// navigation properties.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IRequired<out TEntity, out TValue> : IProperty<TEntity, TValue>
        where TEntity : IEntity
        where TValue : notnull
    {
        IOptional<TEntity, TValue> AsOptional();
    }
}