namespace OData.Client.Demo
{
    public sealed class Contact : IEntity
    {
        public static readonly EntityType<Contact> EntityType = "contact";
        
        public static readonly Property<Contact, IEntityId<Contact>> ContactId = EntityType.IdPropertyName;
        
        public static readonly Property<Contact, string> EmailAddress = "emailaddress1";
        
        public static readonly Property<Contact, string?> GivenName = "givenname";
        
        public static readonly OptionalRef<Contact, Account> ParentCustomer = "parentcustomerid";
    }
}