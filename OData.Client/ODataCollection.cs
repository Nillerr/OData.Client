using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataCollection<TEntity> : IODataCollection<TEntity> where TEntity : IEntity
    {
        private readonly IValueFormatter _valueFormatter;

        public ODataCollection(EntityName<TEntity> entityName, IValueFormatter valueFormatter)
        {
            EntityName = entityName;
            _valueFormatter = valueFormatter;
        }

        public EntityName<TEntity> EntityName { get; }

        public IODataQuery<TEntity> Find(ODataFilter<TEntity> filter)
        {
            var query = new ODataQuery<TEntity>(EntityName, _valueFormatter);
            return query.Filter(filter);
        }
    }
}