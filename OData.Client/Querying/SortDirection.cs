namespace OData.Client
{
    /// <summary>
    /// Specifies the direction to sort in for <see cref="Sorting{TEntity}"/>.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sorts in ascending order.
        /// </summary>
        Ascending = 1,
        
        /// <summary>
        /// Sorts in descending order.
        /// </summary>
        Descending = -1,
    }
}