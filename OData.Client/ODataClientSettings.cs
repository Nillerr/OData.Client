using System;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataClientSettings
    {
        public ODataClientSettings(
            Uri organizationUri,
            IODataPropertiesFactory propertiesFactory,
            IEntitySerializerFactory entitySerializerFactory,
            IODataHttpClient httpClient
        )
        {
            OrganizationUri = organizationUri;
            PropertiesFactory = propertiesFactory;
            EntitySerializerFactory = entitySerializerFactory;
            HttpClient = httpClient;
        }

        public Uri OrganizationUri { get; set; }
        public IODataPropertiesFactory PropertiesFactory { get; set; }
        public IEntitySerializerFactory EntitySerializerFactory { get; set; }

        public IODataHttpClient HttpClient { get; set; }
        
        public IValueFormatter ValueFormatter { get; set; } = new DefaultValueFormatter();
    }
}