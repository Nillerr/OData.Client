using System.Collections.Generic;

namespace OData.Client
{
    public interface IODataFunctionRequest<TResult>
        where TResult : IEntity
    {
        IEntityType<TResult> EntityType { get; }
        string FunctionName { get; }
        
        IReadOnlyDictionary<string, ODataFunctionRequestArgument> Arguments { get; }
        
        IReadOnlyCollection<ISelectableProperty<TResult>> Selection { get; }
        IReadOnlyCollection<ODataExpansion<TResult>> Expansions { get; }
    }
}