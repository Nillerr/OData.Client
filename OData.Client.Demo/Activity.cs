namespace OData.Client.Demo
{
    public abstract class Activity : IEntity
    {
        public static readonly EntityType<Activity> EntityType = "activity";
        
        public static readonly Property<Activity, IEntityId<Activity>> ActivityId = EntityType.IdPropertyName;
        
        public static readonly RequiredRef<Activity, Contact> Contact = "contact";
        public static readonly Refs<Activity, Contact> Contacts = "contacts";

        public static readonly Property<Activity, string> ActivityTypeCode = "activitytypecode";
        
        public static readonly OptionalRef<Activity, IEntity> RegardingObjectId = "regardingobjectid";
    }
}