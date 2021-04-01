namespace OData.Client
{
    /// <summary>
    /// Specifies formatting options for <see cref="IEntity{TEntity}.ToJson"/>.
    /// </summary>
    public enum Formatting2
    {
        /// <summary>
        /// No special formatting is applied. This is the default.
        /// </summary>
        None = 0,

        /// <summary>
        /// Causes child objects to be indented.
        /// </summary>
        Indented = 1,
    }
}