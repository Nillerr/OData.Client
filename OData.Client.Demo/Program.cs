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
        private const string? AuthorizationToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayIsImtpZCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayJ9.eyJhdWQiOiJodHRwczovL3VuaXZlcnNhbC1yb2JvdHMtdWF0LmNybTQuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMzExYTIyZmQtOTU3Ni00MGFiLTk3N2UtYTJmMGE0ZDJjZDM2LyIsImlhdCI6MTYwOTY3Mzc3MSwibmJmIjoxNjA5NjczNzcxLCJleHAiOjE2MDk2Nzc2NzEsImFpbyI6IkUySmdZTGprY0x4OC9UTzFRTGR3OXBnSHNtOVhBUUE9IiwiYXBwaWQiOiI0ODQ1MTVhNy01ZTM1LTRiM2UtYjM4OS1hMzU3Y2M3MTBhZmMiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8zMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYvIiwib2lkIjoiMTY4YzgyN2QtODFlOC00NjZiLTg5YTEtN2FkMzkwNzY2NTYyIiwicmgiOiIwLkFBQUFfU0lhTVhhVnEwQ1hmcUx3cE5MTk5xY1ZSVWcxWGo1THM0bWpWOHh4Q3Z4NUFBQS4iLCJzdWIiOiIxNjhjODI3ZC04MWU4LTQ2NmItODlhMS03YWQzOTA3NjY1NjIiLCJ0aWQiOiIzMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYiLCJ1dGkiOiJhU1dBOVRPNnpVNl9zbnBfMG04RUFBIiwidmVyIjoiMS4wIn0.JE2tVqHbid_ysk8R0pi77M1-pow3fjCMGQUohXck3qX2zEiLWLjp1UYt5o2573AEkY4yyjo3sCb4d-dtYOpuZUicnBLNOaqZm0AYcnRw1doP-16cFzzJJC3DLvm874Oqxzri1lypjXx01_jq8aHFZfdgaWxjmjDVfsJ2EmPQlQ2rHUD1dpVwO8LPBwGKu6vVV8Pt5K3Y9_sYqOm2EBKXuSs90t-Iru7p9qOxlik001avXKfXliAIf5yrAto_8oZln4OJqDtbOwdDhcbdcryC9pqMdF2YrQHu19c8xxCfxJztEoK-yFGx5IZx2dSMWTbs9AXttlFVKOHXz55VK3u4ig";
        
        private static readonly Uri OrganizationUri = new Uri("https://universal-robots-uat.crm4.dynamics.com");

        public static async Task Main(string[] args)
        {
            
            var entityId = "https://universal-robots-uat.crm4.dynamics.com/api/data/v9.0/contacts(6124b4e0-9299-483d-9cf2-5886533215e7)";
            var id = entityId.Substring(entityId.Length - 37, 36);
            Console.WriteLine(id);
            
            var json = "{\"foo\":[{\"k\":\"bar\"}]}";
            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            
            var entity = new JObjectEntity<Incident>(jObject, Incident.EntityName);
            Console.WriteLine(entity.ToJson());

            // var record = JsonConvert.DeserializeObject<MyRecord>("{\"MyUri\":\"https://www.google.com\"}");
            // Console.WriteLine(record!.MyUri.ToString());

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);

            var serializerSettings = new JsonSerializerSettings();
            var serializer = new JsonNetSerializer();
            var propertiesFactory = new JsonNetPropertiesFactory(serializerSettings.Converters);
            
            var valueFormatter = new DefaultValueFormatter();
            
            var oDataHttpClient = new ODataClient(OrganizationUri, httpClient, propertiesFactory, serializer, valueFormatter);
            var incidentCollection = oDataHttpClient.Collection(Incident.EntityName);

            var query = incidentCollection.Find()
                .Filter(Incident.CaseNumber.StartsWith("TS02"))
                .Select(Incident.IncidentId, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact)
                .OrderBy(Incident.CaseNumber)
                .MaxPageSize(1);
            
            Console.WriteLine(query);

            await foreach (var incident in query)
            {
                var incidentId = incident.Value(Incident.IncidentId);
                Console.WriteLine($"[{incidentId.Id:D}]: {incident.ToJson()}");

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