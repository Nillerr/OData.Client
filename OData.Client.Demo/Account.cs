namespace OData.Client.Demo
{
    public sealed class Account : IEntity
    {
        public static readonly EntityType<Account> EntityType = "account";
        
        public static readonly Required<Account, IEntityId<Account>> AccountId = "accountid";
        public static readonly Required<Account, bool> IsAdmin = "is_admin";
    }
}