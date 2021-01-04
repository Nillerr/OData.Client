namespace OData.Client
{
    public readonly struct Sorting<TEntity> where TEntity : IEntity
    {
        public Sorting(IProperty<TEntity> property, SortDirection direction)
        {
            Property = property;
            Direction = direction;
        }

        public IProperty<TEntity> Property { get; }
        public SortDirection Direction { get; }
    }
}