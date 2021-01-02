using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OData.Client.Expressions.Formatting;
using OData.Client.Newtonsoft.Json;

namespace OData.Client.Demo
{
    internal record MyRecord(Uri MyUri);
    
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var json = "{\"foo\":\"bar\"}";
            var entity = JsonConvert.DeserializeObject<JObjectEntity<Incident>>(json);
            Console.WriteLine(entity.ToJson());

            // var record = JsonConvert.DeserializeObject<MyRecord>("{\"MyUri\":\"https://www.google.com\"}");
            // Console.WriteLine(record!.MyUri.ToString());

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);

            var serializer = new JsonNetSerializer();
            var pluralizer = new DefaultPluralizer();

            var valueFormatter = new DefaultValueFormatter();
            var incidentCollection = new ODataCollection<Incident>(Incident.EntityName, valueFormatter, httpClient, serializer, pluralizer);

            var query = incidentCollection.Find()
                .Filter(Incident.CaseNumber.StartsWith("TS02"))
                .Select(Incident.IncidentId, Incident.Title, Incident.CaseNumber, Incident.PrimaryContact)
                .MaxPageSize(1);

            await foreach (var incident in query)
            {
                var jsonResult = JsonConvert.SerializeObject(incident, Formatting.Indented);
                Console.WriteLine(jsonResult);  
            } 

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