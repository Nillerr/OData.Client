namespace OData.Client.Demo
{
    public sealed class Account : IEntity
    {
        public static readonly EntityType<Account> EntityType = "account";
        
        public static readonly Property<Account, IEntityId<Account>> AccountId = EntityType.IdPropertyName;
        
        public static readonly Property<Account, bool> IsAdmin = "is_admin";
    }
}