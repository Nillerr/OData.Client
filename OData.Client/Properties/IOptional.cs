namespace OData.Client
{
    /// <summary>
    /// An optional property is any property that can be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IOptional<out TEntity, out TValue> : IProperty<TEntity, TValue>
        where TEntity : IEntity
        where TValue : notnull
    {
    }
}