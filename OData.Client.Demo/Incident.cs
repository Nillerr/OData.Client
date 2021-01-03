using System;

namespace OData.Client.Demo
{
    public sealed class Incident : IEntity
    {
        private Incident() => throw new InvalidOperationException();

        public static readonly EntityName<Incident> EntityName = "incidents";

        public static readonly Required<Incident, string> ETag = "@odata.etag";
        
        public static readonly Required<Incident, IEntityId<Incident>> IncidentId = "incidentid";
        public static readonly Required<Incident, string> Title = "title";
        public static readonly Optional<Incident, string> CaseNumber = "c2rur_urcasenumber";
        public static readonly Required<Incident, double> DoubleValue = "dbler";
        
        public static readonly OptionalRef<Incident, Contact> PrimaryContact = "primarycontactid";
        // public static readonly OptionalRef<Incident, Contact> SecondaryContact = "secondarycontactid";
        // public static readonly Property<Incident, Contact[]> Contacts = "contacts";
        
        // public static readonly Required<Incident, Activity[]> Activities = "activities";
        // public static readonly Required<Incident, DateTime> CreatedAt = "created_at";
    }
}