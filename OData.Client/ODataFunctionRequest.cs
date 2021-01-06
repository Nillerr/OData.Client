using System.Collections.Generic;
using JetBrains.Annotations;

namespace OData.Client
{
    public sealed class ODataFunctionRequest<[UsedImplicitly] TResult> : IODataFunctionRequest<TResult>
        where TResult : IEntity
    {
        public ODataFunctionRequest(IEntityType<TResult> entityType, string functionName, Dictionary<string, ODataFunctionRequestArgument> arguments)
        {
            EntityType = entityType;
            FunctionName = functionName;
            Arguments = arguments;
        }

        public IEntityType<TResult> EntityType { get; }
        public string FunctionName { get; }
        public Dictionary<string, ODataFunctionRequestArgument> Arguments { get; }
    }
}