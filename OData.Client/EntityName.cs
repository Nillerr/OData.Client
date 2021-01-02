namespace OData.Client
{
    public sealed class EntityName<TEntity> : IEntityName<TEntity> where TEntity : IEntity
    {
        public string Name { get; }

        public EntityName(string name)
        {
            Name = name;
        }

        public static implicit operator EntityName<TEntity>(string str) => new(str);
    }
}