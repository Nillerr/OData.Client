using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace OData.Client
{
    public sealed class DefaultEntitySetNameResolver : IEntitySetNameResolver
    {
        private readonly ConcurrentDictionary<string, AsyncLazy<IEntity<ODataEntityDefinition>>> _cache = new();

        /// <inheritdoc />
        public async Task<string> EntitySetNameAsync<TEntity>(
            ODataMetadataContext<TEntity> context,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var lazy = _cache.GetOrAdd(context.EntityType.Name, ValueFactory, (context, cancellationToken));
            var metadata = await lazy;
            var entitySetName = metadata.Value(ODataEntityDefinition.EntitySetName);
            return entitySetName;
        }

        private AsyncLazy<IEntity<ODataEntityDefinition>> ValueFactory<TEntity>(
            string name,
            (ODataMetadataContext<TEntity> context, CancellationToken cancellationToken) args
        )
            where TEntity : IEntity
        {
            var (context, cancellationToken) = args;
            return new(() => EntityDefinitionAsync(context, cancellationToken), AsyncLazyFlags.RetryOnFailure);
        }

        private async Task<IEntity<ODataEntityDefinition>> EntityDefinitionAsync<TEntity>(
            ODataMetadataContext<TEntity> context,
            CancellationToken cancellationToken = default
        )
            where TEntity : IEntity
        {
            var entityType = ODataEntityDefinition.EntityType;
            
            var arguments = new Dictionary<string, ODataFunctionRequestArgument>();
            arguments.Add("LogicalName", new ODataFunctionRequestArgument(context.EntityType.Name));
            
            var request = new ODataFunctionRequest<ODataEntityDefinition>(entityType, "EntityDefinitions", arguments);
            
            var entityDefinition = await context.ODataClient.InvokeAsync(request, cancellationToken);
            return entityDefinition;
        }
    }
}