﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OData.Client.Expressions.Formatting;
using OData.Client.Newtonsoft.Json;

namespace OData.Client.Demo
{
    internal record MyRecord(Uri MyUri);
    
    public class Program
    {
        private const string? AuthorizationToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayIsImtpZCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayJ9.eyJhdWQiOiJodHRwczovL3VuaXZlcnNhbC1yb2JvdHMtdWF0LmNybTQuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMzExYTIyZmQtOTU3Ni00MGFiLTk3N2UtYTJmMGE0ZDJjZDM2LyIsImlhdCI6MTYwOTYxMzk4NywibmJmIjoxNjA5NjEzOTg3LCJleHAiOjE2MDk2MTc4ODcsImFpbyI6IkUySmdZSGl4dyszL295S05IZnFUbXUvTi9ub2lIZ0E9IiwiYXBwaWQiOiI0ODQ1MTVhNy01ZTM1LTRiM2UtYjM4OS1hMzU3Y2M3MTBhZmMiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8zMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYvIiwib2lkIjoiMTY4YzgyN2QtODFlOC00NjZiLTg5YTEtN2FkMzkwNzY2NTYyIiwicmgiOiIwLkFBQUFfU0lhTVhhVnEwQ1hmcUx3cE5MTk5xY1ZSVWcxWGo1THM0bWpWOHh4Q3Z4NUFBQS4iLCJzdWIiOiIxNjhjODI3ZC04MWU4LTQ2NmItODlhMS03YWQzOTA3NjY1NjIiLCJ0aWQiOiIzMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYiLCJ1dGkiOiJzQmR6ZU0xZm9rLTdHZkFiVjZoLUFBIiwidmVyIjoiMS4wIn0.O5vZGjlkkmCGvoR6rd0bI80nIKrn86tR3HX30i5yBsnUbHLQa0LlqcQvQKc9ebPH2L8BZpzX3k76W0HxRpOVq4-UaZ__wC4cue7M8A6zNp2RnI08S5NaCryxLMETf4re08fZ95ze8_DRJeRpHQZkQHUunydEuKkPnTzspmKLHwbSWiZv6b7go8K4MrlKifrsRT5xJbpAT8liOESkAtn8x0oljcAhrxXDHNFNZdnJVRPrup3b2S6Mo5daYKUR1fRIVkZ9wqC4rK0_fANMmgo8tRBvTXJ8tAAZyYMwF_297O1uvb5kpDhMoQkTQbVMd48kNQr7f1wNWt5YwjowW8Xv3w";

        public static async Task Main(string[] args)
        {
            var entityId = "https://universal-robots-uat.crm4.dynamics.com/api/data/v9.0/contacts(6124b4e0-9299-483d-9cf2-5886533215e7)";
            var id = entityId.Substring(entityId.Length - 37, 36);
            Console.WriteLine(id);
            
            var json = "{\"foo\":\"bar\"}";
            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            var entity = new JObjectEntity<Incident>(jObject, Incident.EntityName);
            Console.WriteLine(entity.ToJson());

            // var record = JsonConvert.DeserializeObject<MyRecord>("{\"MyUri\":\"https://www.google.com\"}");
            // Console.WriteLine(record!.MyUri.ToString());

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);

            var serializer = new JsonNetSerializer();
            var pluralizer = new DefaultPluralizer();

            var valueFormatter = new DefaultValueFormatter();
            var propertiesFactory = new JsonNetPropertiesFactory();
            
            var incidentCollection = new ODataCollection<Incident>(Incident.EntityName, valueFormatter, httpClient, serializer, pluralizer, propertiesFactory);

            var query = incidentCollection.Find()
                .Filter(Incident.CaseNumber.StartsWith("TS02"))
                .Select(Incident.IncidentId, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact)
                .MaxPageSize(1);

            await foreach (var incident in query)
            {
                var incidentId = incident.Value(Incident.IncidentId);
                Console.WriteLine($"[{incidentId.Id:D}]: {incident.ToJson()}");
            }

            await incidentCollection.CreateAsync(e =>
            {
                e.Set(Incident.CaseNumber, "ABC1234");
                e.Bind(Incident.PrimaryContact, Contact.EntityName.ParseId("c6ac128c-83cb-44a8-88af-f4cbb02a8887"));
                e.BindAll(Incident.Activities, new List<IEntityId<Activity>>
                {
                    AdxPortalComment.EntityName.ParseId("a401e907-cd89-4885-b824-ec20d3b6d63d")
                });
            });

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