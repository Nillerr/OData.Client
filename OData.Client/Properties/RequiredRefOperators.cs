namespace OData.Client
{
    public static class RequiredRefOperators
    {
        public static Required<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this RequiredRef<TEntity, TOther> property,
            IRequired<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : notnull
        {
            return $"{property.Name}/{other.Name}";
        }
        public static Optional<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this RequiredRef<TEntity, TOther> property,
            IOptional<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : notnull
        {
            return $"{property.Name}/{other.Name}";
        }

        public static RequiredRef<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this RequiredRef<TEntity, TOther> property,
            IRequiredRef<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : IEntity
        {
            return RequiredRef<TEntity, TValue>.Prefixed($"{property.Name}/", other.Name);
        }

        public static OptionalRef<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this RequiredRef<TEntity, TOther> property,
            IOptionalRef<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : IEntity
        {
            return OptionalRef<TEntity, TValue>.Prefixed($"{property.Name}/", other.Name);
        }
    }
}