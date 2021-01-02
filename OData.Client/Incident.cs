using System;
using System.Text.Json.Serialization;

namespace OData.Client
{
    public sealed class Incident : IEntity
    {
        public static readonly EntityName<Incident> EntityName = "incident";
        
        public static readonly Property<Incident, Guid> IncidentId = "incidentid";
        public static readonly Property<Incident, string> Title = "title";
        public static readonly Property<Incident, string?> CaseNumber = "casenumber";
        
        public static readonly Property<Incident, Contact?> PrimaryContact = "primarycontactid";
        public static readonly Property<Incident, Contact[]> Contacts = "contacts";
    }
}