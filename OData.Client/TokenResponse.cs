using System.Text.Json.Serialization;

namespace OData.Client
{
    public sealed class TokenResponse
    {
        public TokenResponse(
            string tokenType,
            string expiresIn,
            string extExpiresIn,
            string expiresOn,
            string notBefore,
            string resource,
            string accessToken
        )
        {
            TokenType = tokenType;
            ExpiresIn = expiresIn;
            ExtExpiresIn = extExpiresIn;
            ExpiresOn = expiresOn;
            NotBefore = notBefore;
            Resource = resource;
            AccessToken = accessToken;
        }

        [JsonPropertyName("token_type")]
        public string TokenType { get; }
        
        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; }
        
        [JsonPropertyName("ext_expires_in")]
        public string ExtExpiresIn { get; }
        
        [JsonPropertyName("expires_on")]
        public string ExpiresOn { get; }
        
        [JsonPropertyName("not_before")]
        public string NotBefore { get; }
        
        [JsonPropertyName("resource")]
        public string Resource { get; }
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; }
    }
}