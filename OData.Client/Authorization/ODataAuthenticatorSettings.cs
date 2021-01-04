namespace OData.Client
{
    public sealed class ODataAuthenticatorSettings
    {
        public string TenantId { get; set; } = null!;
        public string Resource { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}