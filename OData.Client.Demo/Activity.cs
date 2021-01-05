namespace OData.Client.Demo
{
    public abstract class Activity : IEntity
    {
        public static readonly Required<Activity, IEntityId<Activity>> ActivityId = "activityid";
        public static readonly RequiredRef<Activity, Contact> Contact = "contact";
        public static readonly Refs<Activity, Contact> Contacts = "contacts";

        public static readonly Required<Activity, string> ActivityTypeCode = "activitytypecode";
        
        public static readonly OptionalRef<Activity, IEntity> RegardingObjectId = "regardingobjectid";
    }
}