namespace OData.Client.Demo
{
    public abstract class Activity : IEntity
    {
        public static readonly EntityType<Activity> EntityType = "activities";

        public static readonly Required<Activity, IEntityId<Activity>> ActivityId = "activityid";
        public static readonly RequiredRef<Activity, Contact> Contact = "contact";
        public static readonly Refs<Activity, Contact> Contacts = "contacts";
    }
}