using System;

namespace OData.Client
{
    /// <summary>
    /// An authorization token used when interacting with the OData API.
    /// </summary>
    public sealed class AuthorizationToken
    {
        public AuthorizationToken(string tokenType, DateTime expiresOnUtc, string accessToken)
        {
            TokenType = tokenType;
            ExpiresOnUtc = expiresOnUtc;
            AccessToken = accessToken;
        }

        public string TokenType { get; }
        public DateTime ExpiresOnUtc { get; }
        public string AccessToken { get; }

        public bool IsValidAt(DateTime instant)
        {
            return ExpiresOnUtc > instant;
        }
    }
}