namespace OData.Client
{
    /// <summary>
    /// A non-nullable property of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IRequired<out TEntity, out TValue> : IProperty<TEntity, TValue>
        where TEntity : IEntity
        where TValue : notnull
    {
    }
}