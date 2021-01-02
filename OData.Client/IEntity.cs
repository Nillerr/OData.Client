namespace OData.Client
{
    /// <summary>
    /// Marker interface that enables type safety with usage of <see cref="PropertyOperators.Filter{TEntity,TOther,TValue}"/>
    /// </summary>
    public interface IEntity
    {
    }
    
    public interface IEntity<TEntity> where TEntity : IEntity
    {
        bool Contains(IProperty<TEntity> property);
        
        /// <summary>
        /// Tries to get the value with the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns><see langword="true"/> if a value was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        bool TryGetValue<TValue>(Property<TEntity, TValue> property, out TValue value);

        /// <summary>
        /// Gets the value with the specified <paramref name="property"/> converted to the specified type.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <typeparam name="TValue">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        TValue Value<TValue>(Property<TEntity, TValue> property);

        string ToJson();
    }
}