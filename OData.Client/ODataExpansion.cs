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
        public static ODataExpansion<TEntity> Create<TEntity, TOther>(IProperty<TEntity, TOther?> property)
            where TEntity : IEntity
            where TOther : IEntity
        {
            return new ODataExpansion<TEntity>(property);
        }
    }
}