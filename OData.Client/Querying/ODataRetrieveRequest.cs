using System.Collections.Generic;

namespace OData.Client
{
    /// <inheritdoc />
    public sealed class ODataRetrieveRequest<TEntity> : IODataSelection<TEntity>
        where TEntity : IEntity
    {
        private readonly List<ISelectableProperty<TEntity>> _selection = new();
        private readonly List<ODataExpansion<TEntity>> _expansions = new();

        /// <summary>
        /// The selection to apply.
        /// </summary>
        public IEnumerable<ISelectableProperty<TEntity>> Selection => _selection;

        /// <summary>
        /// The expansions to apply.
        /// </summary>
        public IEnumerable<ODataExpansion<TEntity>> Expansions => _expansions;

        /// <inheritdoc />
        public IODataSelection<TEntity> Select(ISelectableProperty<TEntity> property)
        {
            _selection.Add(property);
            return this;
        }

        /// <inheritdoc />
        public IODataSelection<TEntity> Expand(IExpandableProperty<TEntity> property)
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }
    }
}