namespace OData.Client.Demo
{
    public abstract class Activity : IEntity
    {
        public static readonly EntityName<Activity> EntityName = "activities";

        public static readonly Required<Activity, IEntityId<Activity>> ActivityId = "activityid";
        public static readonly Required<Activity, Contact> Contact = "contact";
    }
}