using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;

namespace OData.Client
{
    public sealed class CRMEntitySetNameResolver : IEntitySetNameResolver
    {
        private readonly ConcurrentDictionary<string, AsyncLazy<IEntity<ODataEntityDefinition>>> _cache = new();

        private readonly ILogger<CRMEntitySetNameResolver> _logger;

        public CRMEntitySetNameResolver(ILogger<CRMEntitySetNameResolver> logger)
        {
            _logger = logger;
        }

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
            _logger.LogDebug("Fetching entity definition for '{EntityType}'...", context.EntityType.Name);
            var entityType = ODataEntityDefinition.EntityType;
            
            var request = ODataFunctionRequest.For(entityType, "EntityDefinitions")
                .Pass("LogicalName", context.EntityType.Name)
                .Select(ODataEntityDefinition.EntitySetName);
            
            var entityDefinition = await context.ODataClient.InvokeAsync(request, cancellationToken);
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Fetched entity definition for '{EntityType}': {EntityDefinition}", context.EntityType.Name, entityDefinition.ToJson(Formatting.None));
            }
            
            return entityDefinition;
        }
    }
}