namespace OData.Client
{
    public static class EntityReferenceExtensions
    {
        public static Required<TEntity, IEntityId<TOther>> Value<TEntity, TOther>(this IRequiredRef<TEntity, TOther> source)
            where TEntity : IEntity
            where TOther : IEntity
        {
            return new Required<TEntity, IEntityId<TOther>>($"_{source.Name}_value");
        }
        
        public static Optional<TEntity, IEntityId<TOther>> Value<TEntity, TOther>(this IOptionalRef<TEntity, TOther> source)
            where TEntity : IEntity
            where TOther : IEntity
        {
            return new Optional<TEntity, IEntityId<TOther>>($"_{source.Name}_value");
        }
    }
}