using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OData.Client.Expressions.Formatting;
using OData.Client.Json.Net;

namespace OData.Client.Demo
{
    internal record MyRecord(Uri MyUri);
    
    public class Program
    {
        private const string? AuthorizationToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayIsImtpZCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayJ9.eyJhdWQiOiJodHRwczovL3VuaXZlcnNhbC1yb2JvdHMtdWF0LmNybTQuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMzExYTIyZmQtOTU3Ni00MGFiLTk3N2UtYTJmMGE0ZDJjZDM2LyIsImlhdCI6MTYwOTY5NTYxOSwibmJmIjoxNjA5Njk1NjE5LCJleHAiOjE2MDk2OTk1MTksImFpbyI6IkUySmdZTmovZlFzajF4MFhjWm5uNmJ2ZjJON2FEUUE9IiwiYXBwaWQiOiI0ODQ1MTVhNy01ZTM1LTRiM2UtYjM4OS1hMzU3Y2M3MTBhZmMiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8zMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYvIiwib2lkIjoiMTY4YzgyN2QtODFlOC00NjZiLTg5YTEtN2FkMzkwNzY2NTYyIiwicmgiOiIwLkFBQUFfU0lhTVhhVnEwQ1hmcUx3cE5MTk5xY1ZSVWcxWGo1THM0bWpWOHh4Q3Z4NUFBQS4iLCJzdWIiOiIxNjhjODI3ZC04MWU4LTQ2NmItODlhMS03YWQzOTA3NjY1NjIiLCJ0aWQiOiIzMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYiLCJ1dGkiOiI5NXRxZWJlcERVSzFvWWRRaWtNTkFBIiwidmVyIjoiMS4wIn0.e751m-FMcueJLTvFm5YAYVxdZFC99PUGp_76q_690UCGEAGZZ5NBDr5OGFBtFZFqqr6wsasJ7PBOVoxF_OcUS_l45Sl3kf7S1s0DPQ8Fcnz_ad-NpJHfqtBhux_ysIPrYoJOT9RRmh3nhvPXBTWfGkOgEGK_xrmAnuq9EKXvTRBdWFuRz5gO3xGmJxMxlZnsGOb6vYExVXmQefRjQVYxqhAWOD0MdQsmT5vceqCXEzmWMtKxeYBx6jCs7kSl9rag4dh5xe_xORy0-n940C3Hyc_oJ8JN4kfMwmAi1bRmHL68o9-bOSqJNOjY7dzIQt8U1FzghG6nmkrkwvcEg4r-jA";
        
        private static readonly Uri OrganizationUri = new Uri("https://universal-robots-uat.crm4.dynamics.com");

        public static async Task Main(string[] args)
        {
            
            // var entityId = "https://universal-robots-uat.crm4.dynamics.com/api/data/v9.0/contacts(6124b4e0-9299-483d-9cf2-5886533215e7)";
            // var id = entityId.Substring(entityId.Length - 37, 36);
            // Console.WriteLine(id);
            //
            // var json = "{\"foo\":[{\"k\":\"bar\"}]}";
            // var jObject = JsonConvert.DeserializeObject<JObject>(json);
            //
            // var entity = new JObjectEntity<Incident>(jObject);
            // Console.WriteLine(entity.ToJson());

            // var record = JsonConvert.DeserializeObject<MyRecord>("{\"MyUri\":\"https://www.google.com\"}");
            // Console.WriteLine(record!.MyUri.ToString());

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);

            var serializerSettings = new JsonSerializerSettings();
            var serializer = new JsonNetSerializerFactory();
            var propertiesFactory = new JsonNetPropertiesFactory(serializerSettings.Converters);
            
            var valueFormatter = new DefaultValueFormatter();
            
            var oDataHttpClient = new ODataClient(OrganizationUri, httpClient, propertiesFactory, serializer, valueFormatter);
            var incidentCollection = oDataHttpClient.Collection(Incident.EntityName);

            var accountId = Account.EntityName.ParseId("1f2a95a3-d251-e711-8107-5065f38bf3a1");
            
            var query = incidentCollection.Find()
                .Filter(
                    Incident.CaseNumber.StartsWith("TS02")
                    & Incident.PrimaryContact.Where(Contact.ParentCustomer) == accountId
                    & Incident.PrimaryContact.Where(Contact.ParentCustomer).IsNull()
                    & Incident.PrimaryContact.Where(Contact.EmailAddress) == "nije@universal-robots.com"
                )
                .Select(Incident.IncidentId, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact.Value())
                .Expand(Incident.PrimaryContact)
                .OrderBy(Incident.CaseNumber)
                .MaxPageSize(1);
            
            Console.WriteLine(query);
            
            await foreach (var incident in query)
            {
                var incidentId = incident.Value(Incident.IncidentId);
                Console.WriteLine($"[{incidentId.Id:D}]: {incident.ToJson()}");

                var primaryContactId = incident.Reference(Incident.PrimaryContact, Contact.EntityName);
                var primaryContact = incident.Entity(Incident.PrimaryContact, Contact.EntityName)!;
                
                var converters = new JsonConverter[]
                {
                    new EntityIdConverter<Incident>(Incident.EntityName),
                    new EntityIdConverter<Contact>(Contact.EntityName)
                };

                var obj = new
                {
                    incidentId,
                    primaryContactId,
                    primaryContact = JsonConvert.DeserializeObject<JObject>(primaryContact.ToJson(), converters)
                };

                var json = JsonConvert.SerializeObject(obj, Formatting.Indented, converters);
                Console.WriteLine(json);

                break;

                // var portalCommentId = AdxPortalComment.EntityName.ParseId("1e65cb68-39c2-41a8-b76b-2cf99192ef6c");
                // await incidentCollection.AssociateAsync(incidentId, Incident.Activities, portalCommentId);
            }

            // await incidentCollection.CreateAsync(e =>
            // {
            //     e.Set(Incident.CaseNumber, "ABC1234");
            //     e.Bind(Incident.PrimaryContact, Contact.EntityName.ParseId("c6ac128c-83cb-44a8-88af-f4cbb02a8887"));
            //     e.BindAll(Incident.Activities, new List<IEntityId<Activity>>
            //     {
            //         AdxPortalComment.EntityName.ParseId("a401e907-cd89-4885-b824-ec20d3b6d63d")
            //     });
            // });

            // var baseUri = new Uri("https://microsoft.com/api/data/v9.1");
            // var entityUri = new Uri(baseUri, "accounts");
            // var requestUriBuilder = new UriBuilder(entityUri);
            // requestUriBuilder.Query = query;
            //
            // var requestUri = requestUriBuilder.Uri;
            // Console.WriteLine(requestUri);
        }
    }
}