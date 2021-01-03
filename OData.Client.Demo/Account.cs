namespace OData.Client.Demo
{
    public sealed class Account : IEntity
    {
        public static readonly EntityName<Account> EntityName = "accounts";
        
        public static readonly Required<Account, IEntityId<Account>> AccountId = "accountid";
    }
}