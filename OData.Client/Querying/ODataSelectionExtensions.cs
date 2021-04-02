using System.Linq;

namespace OData.Client
{
    /// <summary>
    /// Extension methods for selection builders.
    /// </summary>
    public static class ODataSelectionExtensions
    {
        /// <summary>
        /// Specifies properties to select.
        /// </summary>
        /// <param name="selection">The selection builder.</param>
        /// <param name="properties">The properties to select.</param>
        /// <returns>The selection builder.</returns>
        public static IODataSelection<TEntity> Select<TEntity>(
            this IODataSelection<TEntity> selection,
            params ISelectableProperty<TEntity>[] properties
        )
            where TEntity : IEntity
        {
            return properties.Aggregate(selection, (current, property) => current.Select(property));
        }

        /// <summary>
        /// Specifies properties to expand.
        /// </summary>
        /// <param name="selection">The selection builder.</param>
        /// <param name="properties">The properties to expand.</param>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The selection builder.</returns>
        public static IODataSelection<TEntity> Expand<TEntity>(
            this IODataSelection<TEntity> selection,
            params IExpandableProperty<TEntity>[] properties
        )
            where TEntity : IEntity
        {
            return properties.Aggregate(selection, (current, property) => current.Expand(property));
        }
    }
}