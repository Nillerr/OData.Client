namespace OData.Client.Demo
{
    public sealed class Contact : IEntity
    {
        public static readonly EntityName<Contact> EntityName = "contacts";
        
        public static readonly Property<Contact, string> EmailAddress = "emailaddress1";
    }
}