using System;

namespace OData.Client.Demo
{
    public sealed class Incident : IEntity
    {
        private Incident() => throw new InvalidOperationException();

        public static readonly EntityType<Incident> EntityType = "incident";
        public static readonly Property<Incident, IEntityId<Incident>> IncidentId = EntityType.IdPropertyName;

        public static readonly Property<Incident, string> ETag = "@odata.etag";
        
        public static readonly Property<Incident, DateTime> CreatedOn = "createdon";
        public static readonly Property<Incident, string> Title = "title";
        public static readonly Property<Incident, string> CaseNumber = "c2rur_urcasenumber";
        public static readonly Property<Incident, double> DoubleValue = "dbler";
        public static readonly Property<Incident, double?> OptionalDoubleValue = "dbler";
        
        public static readonly OptionalRef<Incident, Contact> PrimaryContact = "primarycontactid";
        
        // public static readonly RequiredRef<Incident, Account> PrimaryContact = "customerid";
        
        public static readonly Refs<Incident, Activity> Activities = "activities";
        
        // public static readonly OptionalRef<Incident, Contact> SecondaryContact = "secondarycontactid";
        // public static readonly Property<Incident, Contact[]> Contacts = "contacts";
        
        // public static readonly Required<Incident, Activity[]> Activities = "activities";
        // public static readonly Required<Incident, DateTime> CreatedAt = "created_at";
    }
}