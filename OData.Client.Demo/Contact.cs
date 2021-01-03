namespace OData.Client.Demo
{
    public sealed class Contact : IEntity
    {
        public static readonly EntityName<Contact> EntityName = "contacts";
        
        public static readonly Required<Contact, IEntityId<Contact>> ContactId = "contactid";
        public static readonly Required<Contact, string> EmailAddress = "emailaddress1";
        
        public static readonly OptionalRef<Contact, Account> ParentCustomer = "parentcustomerid";
    }
}