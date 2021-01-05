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
        /// <summary>
        /// Returns the name of the single-value navigation property as is is used in <c>$select=</c> expressions,
        /// meaning it will be reformatted as: <c>"_{property.Name}_value"</c>.
        /// </summary>
        /// <returns>The value-name of the property.</returns>
        string ValueName { get; }
    }
}