namespace OData.Client.Demo
{
    public abstract class Activity : IEntity
    {
        public static readonly EntityName<Activity> EntityName = "activities";

        public static readonly Property<Activity, Contact> Contact = "contact";
    }
}