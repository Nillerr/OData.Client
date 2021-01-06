using System.Collections.Generic;
using JetBrains.Annotations;

namespace OData.Client
{
    public sealed class ODataFunctionRequest<[UsedImplicitly] TResult> :
        IODataFunctionRequest<TResult>,
        IODataSelection<TResult>
        where TResult : IEntity
    {
        private readonly List<ISelectableProperty<TResult>> _selection = new();
        private readonly List<ODataExpansion<TResult>> _expansions = new();
        
        public ODataFunctionRequest(IEntityType<TResult> entityType, string functionName)
        {
            EntityType = entityType;
            FunctionName = functionName;
        }

        public object this[string parameterName]
        {
            set => Arguments[parameterName] = new ODataFunctionRequestArgument(value);
        }

        public IEntityType<TResult> EntityType { get; }
        public string FunctionName { get; }

        public Dictionary<string, ODataFunctionRequestArgument> Arguments { get; } = new();

        /// <summary>
        /// The selection to apply.
        /// </summary>
        public IEnumerable<ISelectableProperty<TResult>> Selection => _selection;

        /// <summary>
        /// The expansions to apply.
        /// </summary>
        public IEnumerable<ODataExpansion<TResult>> Expansions => _expansions;

        /// <inheritdoc />
        public IODataSelection<TResult> Select(ISelectableProperty<TResult> property)
        {
            _selection.Add(property);
            return this;
        }

        /// <inheritdoc />
        public IODataSelection<TResult> Expand(IExpandableProperty<TResult> property)
        {
            var expansion = ODataExpansion.Create(property);
            _expansions.Add(expansion);
            return this;
        }
        
        /// <summary>
        /// Returns a query string representation of the request using the specified formatting.
        /// </summary>
        /// <param name="formatting">The formatting to apply to the query string.</param>
        /// <returns>The query string representation of the request.</returns>
        public string ToQueryString(QueryStringFormatting formatting)
        {
            var parts = new List<string>(3);

            parts.AddSelection(Selection, formatting);
            parts.AddExpansions(Expansions, formatting);

            var queryString = string.Join("&", parts);
            return queryString;
        }
    }
}