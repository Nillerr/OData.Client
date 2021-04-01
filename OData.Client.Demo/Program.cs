using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OData.Client.Authentication.Microsoft;
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
        private static readonly IEntityId<Incident> IncidentId = Incident.EntityType.ParseId("9c327cfd-4c67-4c76-8530-3657dd4c3853");

        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var services = new ServiceCollection();
            services.Configure<ODataAuthenticatorSettings>(configuration.GetSection("OData").GetSection("Authenticator"));
            services.AddLogging(options => options
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
            );

            await using var serviceProvider = services.BuildServiceProvider();

            var clock = new SystemClock();
            var httpClientProvider = new DefaultHttpClientProvider();

            var authenticatorOptions = serviceProvider.GetRequiredService<IOptions<ODataAuthenticatorSettings>>();
            var authenticator = new ODataMicrosoftAuthenticator(clock, httpClientProvider, authenticatorOptions);

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
                httpClient: oDataHttpClient,
                loggerFactory: serviceProvider.GetRequiredService<ILoggerFactory>()
            );
            
            var oDataClient = new ODataClient(oDataClientSettings);
            var incidents = oDataClient.Collection(Incident.EntityType);

            var adxPortalComments = oDataClient.Collection(AdxPortalComment.EntityType);
            await QueryPortalComments(adxPortalComments);
            // await QueryIncidents(incidents);
            // await QueryIncidents(incidents);
            // await CreateCaseAsync(incidents);
        }

        private static async Task QueryPortalComments(IODataCollection<Activity> adxPortalComments)
        {
            var activities = await adxPortalComments
                .Where(Activity.RegardingObjectId == IncidentId & Activity.ActivityTypeCode == AdxPortalComment.EntityType.Name)
                .Select(Activity.ActivityId, Activity.ActivityTypeCode, Activity.RegardingObjectId)
                .Expand(Activity.RegardingObjectId, Incident.EntityType)
                .ToListAsync();

            foreach (var activity in activities)
            {
                var activityId = activity.Id(Activity.ActivityId);
                Console.WriteLine($"{activityId}: {activity.ToJson(Formatting.Indented)}");
            }
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

            var dateTime = DateTime.Parse("2020-05-18T01:26:32Z");

            string? caseNumber = null;
            var query = incidents
                .Where(Incident.IncidentId == IncidentId)
                .Select(Incident.CreatedOn, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact)
                .Expand(Incident.PrimaryContact)
                .Limit(1);
            
            Console.WriteLine(query);

            var pageNumber = 0;
            await foreach (var page in query.FastPages(maxPageSize: 10))
            {
                pageNumber++;
                Console.WriteLine($"Fetched page #{pageNumber}");
                
                foreach (var incident in page)
                {
                    var incidentId = incident.Id(Incident.IncidentId);
                    Console.WriteLine($"{incidentId}: {incident.ToJson(Formatting.Indented)}");
                }
                
                Console.WriteLine();
            }
        }
    }
}