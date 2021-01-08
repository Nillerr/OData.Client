using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;

namespace OData.Client.Authentication.Microsoft
{
    /// <summary>
    /// Authenticates requests using OAuth 2.0 with <c>login.microsoftonline.com</c>.
    /// </summary>
    [PublicAPI]
    public sealed class ODataMicrosoftAuthenticator : IODataAuthenticator
    {
        private readonly AsyncLock _lock = new AsyncLock();
        
        private readonly IClock _clock;
        private readonly IHttpClientProvider _httpClientProvider;
        
        private readonly IOptions<ODataAuthenticatorSettings> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataMicrosoftAuthenticator"/> class.
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="httpClientProvider"></param>
        /// <param name="options"></param>
        public ODataMicrosoftAuthenticator(
            IClock clock,
            IHttpClientProvider httpClientProvider,
            IOptions<ODataAuthenticatorSettings> options
        )
        {
            _clock = new UtcClock(clock);
            _httpClientProvider = httpClientProvider;
            _options = options;
        }

        private ODataAuthenticatorSettings Options => _options.Value;

        private AuthorizationToken? AuthorizationToken { get; set; }

        /// <inheritdoc />
        public async Task AuthorizeAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default
        )
        {
            var authorizationToken = await AcquireAuthorizationTokenAsync(cancellationToken);
            
            var tokenType = authorizationToken.TokenType;
            var accessToken = authorizationToken.AccessToken;
            request.Headers.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
        }

        private async Task<AuthorizationToken> AcquireAuthorizationTokenAsync(CancellationToken cancellationToken)
        {
            using (await _lock.LockAsync(cancellationToken))
            {
                var authorizationToken = AuthorizationToken;
                if (authorizationToken != null && authorizationToken.IsValidAt(_clock.UtcNow))
                    return authorizationToken;
                
                authorizationToken = await AuthenticateAsync(cancellationToken);
                AuthorizationToken = authorizationToken;

                return authorizationToken;
            }
        }

        private async Task<AuthorizationToken> AuthenticateAsync(CancellationToken cancellationToken)
        {
            var requestUri = new Uri($"https://login.microsoftonline.com/{Options.TenantId}/oauth2/token");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var formData = new Dictionary<string, string>();
            formData["resource"] = Options.Resource;
            formData["client_id"] = Options.ClientId;
            formData["client_secret"] = Options.ClientSecret;
            formData["grant_type"] = "client_credentials";

            httpRequest.Content = new FormUrlEncodedContent(formData!);

            var httpClient = _httpClientProvider.HttpClient;
            
            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            // TODO @nije: Throw an exception including the content
            httpResponse.EnsureSuccessStatusCode();

            var stringContent = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
            
            var token = await JsonSerializer.DeserializeAsync<TokenResponse>(stringContent, cancellationToken: cancellationToken);
            if (token == null)
            {
                // TODO @nije: Throw a good exception
                throw new NotImplementedException();
            }

            var expiresOnUtc = long.Parse(token.ExpiresOn);
            var expiresOn = DateTimeOffset.FromUnixTimeSeconds(expiresOnUtc);
            var authorizationToken = new AuthorizationToken(token.TokenType, expiresOn.UtcDateTime, token.AccessToken);
            return authorizationToken;
        }
    }
}