using System;
using System.Text.Json.Serialization;

namespace OData.Client
{
    public sealed class Incident : IEntity
    {
        public static readonly EntityName<Incident> EntityName = "incident";
        
        public static readonly Field<Incident, Guid> IncidentId = "incidentid";
        public static readonly Field<Incident, string> Title = "title";
        public static readonly Field<Incident, string?> CaseNumber = "casenumber";
        
        public static readonly Field<Incident, Contact?> PrimaryContact = "primarycontactid";
        public static readonly Field<Incident, Contact[]> Contacts = "contacts";
    }
}