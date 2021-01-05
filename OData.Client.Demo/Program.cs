using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OData.Client.Json.Net;

namespace OData.Client.Demo
{
    internal record MyRecord(Uri MyUri);

    public enum MyEnum
    {
        Foo = 3,
        Bar = 5,
    }
    
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var services = new ServiceCollection();
            services.Configure<ODataAuthenticatorSettings>(configuration.GetSection("OData").GetSection("Authenticator"));

            await using var serviceProvider = services.BuildServiceProvider();

            var clock = new SystemClock();
            var httpClientProvider = new DefaultHttpClientProvider();

            var authenticatorOptions = serviceProvider.GetRequiredService<IOptions<ODataAuthenticatorSettings>>();
            var authenticator = new ODataAuthenticator(clock, httpClientProvider, authenticatorOptions);

            var defaultODataHttpClient = new DefaultODataHttpClient(httpClientProvider);
            var authenticatedODataHttpClient = new AuthenticatedODataHttpClient(authenticator, defaultODataHttpClient);
            var rateLimitPolicy = new DefaultRateLimitPolicy();
            var oDataHttpClient = new RateLimitedODataHttpClient(clock, authenticatedODataHttpClient, rateLimitPolicy);

            var organizationUri = new Uri(authenticatorOptions.Value.Resource);
            
            var serializerSettings = new JsonSerializerSettings();
            var entitySerializer = new JsonNetEntitySerializer();
            var propertiesFactory = new JsonNetPropertiesFactory(serializerSettings);
            
            var oDataClientSettings = new ODataClientSettings(
                organizationUri: organizationUri,
                propertiesFactory: propertiesFactory,
                entitySerializer: entitySerializer,
                httpClient: oDataHttpClient
            );
            
            var oDataClient = new ODataClient(oDataClientSettings);
            var incidents = oDataClient.Collection(Incident.EntityType);

            await QueryIncidents(incidents);
            // await CreateCaseAsync(incidents);
        }

        private static async Task CreateCaseAsync(IODataCollection<Incident> incidents)
        {
            var id = await incidents.CreateAsync(e =>
            {
                e.Set(Incident.CaseNumber, "ABC1234");
                e.Bind(Incident.PrimaryContact, Contact.EntityType.ParseId("c6ac128c-83cb-44a8-88af-f4cbb02a8887"));
                e.BindAll(
                    Incident.Activities,
                    new List<IEntityId<Activity>>
                    {
                        AdxPortalComment.EntityType.ParseId("a401e907-cd89-4885-b824-ec20d3b6d63d")
                    }
                );
            });
            
            Console.WriteLine(id);
        }

        private static async Task QueryIncidents(IODataCollection<Incident> incidents)
        {
            var accountId = Account.EntityType.ParseId("1f2a95a3-d251-e711-8107-5065f38bf3a1");

            var query = incidents
                .Where(
                    Incident.CaseNumber.StartsWith("TS02")
                    & Incident.PrimaryContact.Where(Contact.ParentCustomer).IsNotNull()
                    & Incident.PrimaryContact.Where(Contact.ParentCustomer) == accountId
                    & Incident.PrimaryContact.Where(Contact.EmailAddress) == "support.na@universal-robots.com"
                )
                .Select(Incident.IncidentId, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact)
                // .Expand(Incident.PrimaryContact)
                .OrderBy(Incident.CaseNumber)
                .Limit(3);
            
            Console.WriteLine(query);

            var pageNumber = 0;
            await foreach (var page in query.FastPages(2))
            {
                pageNumber++;
                Console.WriteLine($"Fetched page #{pageNumber}");
                
                foreach (var incident in page)
                {
                    var incidentId = incident.Id(Incident.IncidentId);
                    Console.WriteLine($"[{incidentId}]: {incident.ToJson(Formatting.Indented)}");
                }
                
                Console.WriteLine();
            }
        }
    }
}