using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace OData.Client
{
    public sealed class ODataAuthenticator : IODataAuthenticator
    {
        private readonly AsyncLock _lock = new AsyncLock();
        
        private readonly IClock _clock;
        private readonly IHttpClientProvider _httpClientProvider;

        public ODataAuthenticator(IClock clock, IHttpClientProvider httpClientProvider)
        {
            _clock = new UtcClock(clock);
            _httpClientProvider = httpClientProvider;
        }

        public AuthorizationToken? AuthorizationToken { get; set; }

        public async Task AddAuthenticationAsync(
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
            var authRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

            var formData = new Dictionary<string, string>();
            formData["resource"] = serviceUrl.ToString();
            formData["client_id"] = clientId;
            formData["client_secret"] = clientSecret;
            formData["grant_type"] = "client_credentials";

            authRequest.Content = new FormUrlEncodedContent(formData!);

            var httpClient = _httpClientProvider.HttpClient;
            var response = await httpClient.SendAsync(authRequest, cancellationToken);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStreamAsync(cancellationToken);
            
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