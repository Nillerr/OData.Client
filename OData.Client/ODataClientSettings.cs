using System;
using Microsoft.Extensions.Logging;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataClientSettings
    {
        private IEntitySetNameResolver? _entitySetNameResolver;

        public ODataClientSettings(
            Uri organizationUri,
            IODataPropertiesFactory propertiesFactory,
            IEntitySerializer entitySerializer,
            IODataHttpClient httpClient,
            ILoggerFactory loggerFactory
        )
        {
            OrganizationUri = organizationUri;
            PropertiesFactory = propertiesFactory;
            EntitySerializer = entitySerializer;
            HttpClient = httpClient;
            LoggerFactory = loggerFactory;
        }

        public Uri OrganizationUri { get; set; }
        public IODataPropertiesFactory PropertiesFactory { get; set; }
        public IEntitySerializer EntitySerializer { get; set; }
        public IODataHttpClient HttpClient { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        
        public IValueFormatter ValueFormatter { get; set; } = new DefaultValueFormatter();

        public IEntitySetNameResolver EntitySetNameResolver
        {
            get => GetOrCreateDefault(ref _entitySetNameResolver, () =>
            {
                var logger = LoggerFactory.CreateLogger<CRMEntitySetNameResolver>();
                return new CRMEntitySetNameResolver(logger);
            });
            set => _entitySetNameResolver = value;
        }

        private static T GetOrCreateDefault<T>(ref T? property, Func<T> defaultFactory)
            where T : notnull
        {
            var value = property;
            if (value != null)
            {
                return value;
            }

            var defaultValue = defaultFactory();
            property = defaultValue;

            return defaultValue;
        }
    }
}