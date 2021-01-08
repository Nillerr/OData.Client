using System.Collections.Generic;
using JetBrains.Annotations;

namespace OData.Client
{
    public interface IODataFunctionRequest<out TResult>
        where TResult : IEntity
    {
        public IEntityType<TResult> EntityType { get; }
        string FunctionName { get; }
        Dictionary<string, ODataFunctionRequestArgument> Arguments { get; }
    }
}