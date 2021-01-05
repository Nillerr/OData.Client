namespace OData.Client
{
    /// <summary>
    /// A navigation property to expand.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public readonly struct ODataExpansion<TEntity> where TEntity : IEntity
    {
        internal ODataExpansion(IExpandableProperty<TEntity> property)
        {
            Property = property;
        }

        /// <summary>
        /// The navigation property to expand.
        /// </summary>
        public IExpandableProperty<TEntity> Property { get; }
    }

    public static class ODataExpansion
    {
        public static ODataExpansion<TEntity> Create<TEntity>(IExpandableProperty<TEntity> property)
            where TEntity : IEntity
        {
            return new ODataExpansion<TEntity>(property);
        }
    }
}