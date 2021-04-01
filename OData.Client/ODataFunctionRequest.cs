using System;
using System.Collections.Generic;
using System.Linq;

namespace OData.Client
{
    public sealed class ODataFunctionRequest<TResult> :
        IODataFunctionRequest<TResult>,
        IEquatable<ODataFunctionRequest<TResult>>
        where TResult : IEntity
    {
        private readonly List<ISelectableProperty<TResult>> _selection = new();
        private readonly List<ODataExpansion<TResult>> _expansions = new();
        private readonly Dictionary<string, ODataFunctionRequestArgument> _arguments = new();

        public ODataFunctionRequest(IEntityType<TResult> entityType, string functionName)
        {
            EntityType = entityType;
            FunctionName = functionName;
        }

        public object? this[string parameterName]
        {
            get => _arguments[parameterName].Value;
            set => _arguments[parameterName] = new ODataFunctionRequestArgument(value);
        }

        public IEntityType<TResult> EntityType { get; }
        public string FunctionName { get; }

        public IReadOnlyCollection<ISelectableProperty<TResult>> Selection => _selection;
        public IReadOnlyCollection<ODataExpansion<TResult>> Expansions => _expansions;
        public IReadOnlyDictionary<string, ODataFunctionRequestArgument> Arguments => _arguments;

        public ODataFunctionRequest<TResult> Pass(string parameterName, object? value)
        {
            _arguments[parameterName] = new ODataFunctionRequestArgument(value);
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

        public bool Equals(ODataFunctionRequest<TResult>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EntityType.Equals(other.EntityType) &&
                   FunctionName == other.FunctionName &&
                   _arguments.Count == other._arguments.Count &&
                   _arguments
                       .All(argument => Equals(argument.Value, other._arguments.GetValueOrDefault(argument.Key))) &&
                   _selection.Count == other._selection.Count && _selection.All(other._selection.Contains) &&
                   _expansions.Count == other._expansions.Count && _expansions.All(other._expansions.Contains);
        }

        public override bool Equals(object? obj) =>
            ReferenceEquals(this, obj) || obj is ODataFunctionRequest<TResult> other && Equals(other);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(FunctionName);
            hashCode.Add(EntityType);

            foreach (var argument in _arguments)
            {
                hashCode.Add(argument);
            }

            foreach (var selection in _selection)
            {
                hashCode.Add(selection);
            }

            foreach (var expansion in _expansions)
            {
                hashCode.Add(expansion);
            }

            return hashCode.ToHashCode();
        }

        public static bool operator ==(ODataFunctionRequest<TResult>? left, ODataFunctionRequest<TResult>? right) =>
            Equals(left, right);

        public static bool operator !=(ODataFunctionRequest<TResult>? left, ODataFunctionRequest<TResult>? right) =>
            !Equals(left, right);
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