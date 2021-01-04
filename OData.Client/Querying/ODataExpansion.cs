namespace OData.Client
{
    public readonly struct ODataExpansion<TEntity> where TEntity : IEntity
    {
        internal ODataExpansion(IProperty<TEntity> property)
        {
            Property = property;
        }

        public IProperty<TEntity> Property { get; }
    }

    public static class ODataExpansion
    {
        public static ODataExpansion<TEntity> Create<TEntity>(IRefProperty<TEntity> property)
            where TEntity : IEntity
        {
            return new ODataExpansion<TEntity>(property);
        }
    }
}