using System;
using System.Net.Http;
using OData.Client.Expressions.Formatting;

namespace OData.Client
{
    public sealed class ODataClientSettings
    {
        public ODataClientSettings(
            Uri organizationUri,
            IODataPropertiesFactory propertiesFactory,
            IEntitySerializerFactory entitySerializerFactory
        )
        {
            OrganizationUri = organizationUri;
            PropertiesFactory = propertiesFactory;
            EntitySerializerFactory = entitySerializerFactory;
        }

        public Uri OrganizationUri { get; set; }
        public IODataPropertiesFactory PropertiesFactory { get; set; }
        public IEntitySerializerFactory EntitySerializerFactory { get; set; }
        
        public HttpClient? HttpClient { get; set; }
        
        public IValueFormatter ValueFormatter { get; set; } = new DefaultValueFormatter();
    }
}