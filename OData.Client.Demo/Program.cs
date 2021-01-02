using System;
using System.Text.Json;
using OData.Client.Expressions.Formatting;

namespace OData.Client.Demo
{
    record MyRecord(Uri MyUri);
    
    class Program
    {
        static void Main(string[] args)
        {
            // var record = JsonSerializer.Deserialize<MyRecord>("{\"MyUri\":\"https://www.google.com\"}");
            // Console.WriteLine(record!.MyUri.ToString());

            var valueFormatter = new DefaultValueFormatter();
            var incidentCollection = new ODataCollection<Incident>(Incident.EntityName, valueFormatter);

            var any = !Incident.Contacts.Any(Contact.EmailAddress.EndsWith("@universal-robots.com"));

            var oDataFilter = 
                (Incident.Title == "The title" | Incident.Title == "The Title 2") 
                & Incident.PrimaryContact.Filter(Contact.EmailAddress) == "nije@universal-robots.com";
            
            var filter = oDataFilter & any;

            var query = incidentCollection.Find(filter)
                // .Expand(Incident.PrimaryContact)
                // .Select(Incident.CaseNumber)
                .ToQueryString();
            
            Console.WriteLine(query);

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