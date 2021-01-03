using System.Collections.Generic;

namespace OData.Client
{
    public sealed class ODataRetrieveRequest<TEntity> : IODataSelection<TEntity> where TEntity : IEntity
    {
        private readonly List<IProperty<TEntity>> _selection = new();
        private readonly List<ODataExpansion<TEntity>> _expansions = new();

        public IEnumerable<IProperty<TEntity>> Selection => _selection;

        public IEnumerable<ODataExpansion<TEntity>> Expansions => _expansions;

        public IODataSelection<TEntity> Select(IProperty<TEntity> property)
        {
            _selection.Add(property);
            return this;
        }

        public IODataSelection<TEntity> Select(params IProperty<TEntity>[] properties)
        {
            _selection.AddRange(properties);
            return this;
        }

        public IODataSelection<TEntity> Expand<TOther>(Property<TEntity, TOther?> property) where TOther : IEntity
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }
    }
}