namespace OData.Client.Demo
{
    public sealed class Account : IEntity
    {
        public static readonly EntityType<Account> EntityType = "accounts";
        
        public static readonly Required<Account, IEntityId<Account>> AccountId = "accountid";
    }
}