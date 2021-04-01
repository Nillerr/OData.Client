namespace OData.Client.Demo
{
    public sealed class AdxPortalComment
    {
        public static readonly EntityType<Activity> EntityType = Activity.EntityType.Child("adx_portalcomment");
    }
}