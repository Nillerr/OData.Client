namespace OData.Client
{
    /// <summary>
    /// A combination of a property to sort by, and a direction to sort it in.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    [Equals]
    public readonly struct Sorting<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sorting{TEntity}"/> struct.
        /// </summary>
        /// <param name="property">The property to sort by.</param>
        /// <param name="direction">The direction to sort in.</param>
        public Sorting(ISortableProperty<TEntity> property, SortDirection direction)
        {
            Property = property;
            Direction = direction;
        }

        /// <summary>
        /// The property to sort by.
        /// </summary>
        public ISortableProperty<TEntity> Property { get; }
        
        /// <summary>
        /// The direction to sort in.
        /// </summary>
        public SortDirection Direction { get; }

        public override string ToString()
        {
            return $"{nameof(Property)}: {Property}, {nameof(Direction)}: {Direction:G}";
        }
    }
}