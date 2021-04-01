using System;
using Microsoft.Extensions.Logging;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataClientSettings
    {
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
            
            ValueFormatter = new DefaultValueFormatter();
            ExpressionFormatter = new DefaultExpressionFormatter(ValueFormatter);
            
            var logger = loggerFactory.CreateLogger<CRMEntitySetNameResolver>();
            EntitySetNameResolver = new CRMEntitySetNameResolver(logger);
        }

        public Uri OrganizationUri { get; }
        public IODataPropertiesFactory PropertiesFactory { get; }
        public IEntitySerializer EntitySerializer { get; }
        public IODataHttpClient HttpClient { get; }
        public ILoggerFactory LoggerFactory { get; }

        public IValueFormatter ValueFormatter { get; set; }
        public IExpressionFormatter ExpressionFormatter { get; set; }
        public IEntitySetNameResolver EntitySetNameResolver { get; set; }
    }
}