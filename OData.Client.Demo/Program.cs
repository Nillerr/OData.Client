using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OData.Client.Json.Net;

namespace OData.Client.Demo
{
    internal record MyRecord(Uri MyUri);

    public class Program
    {
        private const string? AuthorizationToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayIsImtpZCI6IjVPZjlQNUY5Z0NDd0NtRjJCT0hIeEREUS1EayJ9.eyJhdWQiOiJodHRwczovL3VuaXZlcnNhbC1yb2JvdHMtdWF0LmNybTQuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMzExYTIyZmQtOTU3Ni00MGFiLTk3N2UtYTJmMGE0ZDJjZDM2LyIsImlhdCI6MTYwOTcwMDYyMiwibmJmIjoxNjA5NzAwNjIyLCJleHAiOjE2MDk3MDQ1MjIsImFpbyI6IkUySmdZRmoyL2FUT3A5MXpHMDVmczdtbVgrWThCd0E9IiwiYXBwaWQiOiI0ODQ1MTVhNy01ZTM1LTRiM2UtYjM4OS1hMzU3Y2M3MTBhZmMiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8zMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYvIiwib2lkIjoiMTY4YzgyN2QtODFlOC00NjZiLTg5YTEtN2FkMzkwNzY2NTYyIiwicmgiOiIwLkFBQUFfU0lhTVhhVnEwQ1hmcUx3cE5MTk5xY1ZSVWcxWGo1THM0bWpWOHh4Q3Z4NUFBQS4iLCJzdWIiOiIxNjhjODI3ZC04MWU4LTQ2NmItODlhMS03YWQzOTA3NjY1NjIiLCJ0aWQiOiIzMTFhMjJmZC05NTc2LTQwYWItOTc3ZS1hMmYwYTRkMmNkMzYiLCJ1dGkiOiJxY1VZc3U3dDhVcThuNzMyTmYwTkFBIiwidmVyIjoiMS4wIn0.ANrJMF7kZH2dHC9fx-iJvjbM505H-k6vJ42s7Pu4D6uBjiqANkqjP2cWfSswcf9I9mfB3jOe6rBrOY-cKaDvcsgdcuhs_cFFtAth2DcHaSKAzLz9HpM0PPXMpVGBjSujvhJ3nTDBxWpwKkqwNZPtvoD9vvFIM6oJW6KJyQ3FvuDRkgEwcABzhBn_VhKWkaFSPnmC2QpPZ-uPErYZpfA0nA-Ad3W5JVIJ8YMoAmsp97xoR_pg-iaPIqfDubHL9kanqx28VzZZP1BlHbyDXJhLfnOFJQ95nS64bBuRqh8F3h0HypB-1kBoT_W7nSAUNPR-t50dh6zM8lzwq55EH4bNUw";
        
        private static readonly Uri OrganizationUri = new Uri("https://universal-robots-uat.crm4.dynamics.com");

        public static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);

            var serializerSettings = new JsonSerializerSettings();
            var entitySerializerFactory = new JsonNetEntitySerializerFactory();
            var propertiesFactory = new JsonNetPropertiesFactory(serializerSettings.Converters);
            
            var oDataClientSettings = new ODataClientSettings(OrganizationUri, propertiesFactory, entitySerializerFactory);
            oDataClientSettings.HttpClient = httpClient;
            
            var oDataClient = new ODataClient(oDataClientSettings);
            var incidents = oDataClient.Collection(Incident.EntityName);

            await QueryIncidents(incidents);
            // await CreateCaseAsync(incidents);
        }

        private static async Task CreateCaseAsync(IODataCollection<Incident> incidents)
        {
            var id = await incidents.CreateAsync(e =>
            {
                e.Set(Incident.CaseNumber, "ABC1234");
                e.Bind(Incident.PrimaryContact, Contact.EntityName.ParseId("c6ac128c-83cb-44a8-88af-f4cbb02a8887"));
                e.BindAll(
                    Incident.Activities,
                    new List<IEntityId<Activity>>
                    {
                        AdxPortalComment.EntityName.ParseId("a401e907-cd89-4885-b824-ec20d3b6d63d")
                    }
                );
            });
            
            Console.WriteLine(id);
        }

        private static async Task QueryIncidents(IODataCollection<Incident> incidents)
        {
            var accountId = Account.EntityName.ParseId("1f2a95a3-d251-e711-8107-5065f38bf3a1");

            var query = incidents
                .Where(
                    Incident.CaseNumber.StartsWith("TS02")
                    & Incident.PrimaryContact.Where(Contact.ParentCustomer).IsNotNull()
                    & Incident.PrimaryContact.Where(Contact.ParentCustomer) == accountId
                    & Incident.PrimaryContact.Where(Contact.EmailAddress) == "support.na@universal-robots.com"
                )
                .Select(Incident.IncidentId, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact.Value())
                // .Expand(Incident.PrimaryContact)
                .OrderBy(Incident.CaseNumber)
                .MaxPageSize(2);

            Console.WriteLine(query);

            await foreach (var incident in query.Take(5))
            {
                Console.WriteLine(incident.ToJson());
            }
        }
    }
}