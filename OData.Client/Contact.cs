namespace OData.Client
{
    public interface IEntity
    {
        
    }
    
    public sealed class Contact : IEntity
    {
        public static readonly Field<Contact, string> EmailAddress = "emailaddress1";
    }
}