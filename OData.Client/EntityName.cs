namespace OData.Client
{
    public readonly struct EntityName<TEntity>
    {
        public string Name { get; }

        public EntityName(string name)
        {
            Name = name;
        }

        public static implicit operator EntityName<TEntity>(string str) => new(str);
    }
}