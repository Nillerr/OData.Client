namespace OData.Client.Demo
{
    public sealed class Incident : IEntity
    {
        public static readonly EntityName<Incident> EntityName = "incidents";

        public static readonly Property<Incident, string> ETag = "@odata.etag";
        
        public static readonly Property<Incident, IEntityId<Incident>> IncidentId = "incidentid";
        public static readonly Property<Incident, string> Title = "title";
        public static readonly Property<Incident, string?> CaseNumber = "c2rur_urcasenumber";
        
        public static readonly Property<Incident, Contact?> PrimaryContact = "primarycontactid";
        // public static readonly Property<Incident, Contact[]> Contacts = "contacts";
        
        public static readonly Property<Incident, Activity[]> Activities = "activities";
    }
}