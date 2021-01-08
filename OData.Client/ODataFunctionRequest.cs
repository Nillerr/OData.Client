using System.Collections.Generic;

namespace OData.Client
{
    public sealed class ODataFunctionRequest<TResult> :
        IODataFunctionRequest<TResult>
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

        public ODataFunctionRequest<TResult> Pass(string parameterName, object? value)
        {
            Arguments[parameterName] = new ODataFunctionRequestArgument(value);
            return this;
        }

        /// <inheritdoc />
        public ODataFunctionRequest<TResult> Select(ISelectableProperty<TResult> property)
        {
            _selection.Add(property);
            return this;
        }

        /// <inheritdoc />
        public ODataFunctionRequest<TResult> Expand(IExpandableProperty<TResult> property)
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

    public static class ODataFunctionRequest
    {
        public static ODataFunctionRequest<TEntity> For<TEntity>(IEntityType<TEntity> entityType, string functionName)
            where TEntity : IEntity
        {
            return new ODataFunctionRequest<TEntity>(entityType, functionName);
        }
    }
}