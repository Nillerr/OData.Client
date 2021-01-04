namespace OData.Client
{
    public static class RequiredRefOperators
    {
        public static Required<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this RequiredRef<TEntity, TOther> property,
            Required<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : notnull
        {
            return $"{property.Name}/{other.Name}";
        }

        public static Optional<TEntity, TValue> Where<TEntity, TOther, TValue>(
            this RequiredRef<TEntity, TOther> property,
            Optional<TOther, TValue> other
        )
            where TEntity : IEntity
            where TOther : IEntity
            where TValue : notnull
        {
            return $"{property.Name}/{other.Name}";
        }
    }
}