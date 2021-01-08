using System;

namespace OData.Client.Authentication.Microsoft
{
    /// <summary>
    /// An authorization token used when interacting with the OData API.
    /// </summary>
    internal sealed class AuthorizationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationToken"/> class.
        /// </summary>
        /// <param name="tokenType">The token type.</param>
        /// <param name="expiresOnUtc">The time at which the token expires.</param>
        /// <param name="accessToken">The access token.</param>
        public AuthorizationToken(string tokenType, DateTime expiresOnUtc, string accessToken)
        {
            TokenType = tokenType;
            ExpiresOnUtc = expiresOnUtc;
            AccessToken = accessToken;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TokenType { get; }
        public DateTime ExpiresOnUtc { get; }
        public string AccessToken { get; }

        public bool IsValidAt(DateTime instant)
        {
            return ExpiresOnUtc > instant;
        }
    }
}