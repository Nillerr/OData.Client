namespace OData.Client
{
    public sealed class ODataAuthenticatorSettings
    {
        public string TenantId { get; set; }
        public string Resource { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}