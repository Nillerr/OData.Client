using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;

namespace OData.Client
{
    public sealed class AuthenticatedODataHttpClient : IODataHttpClient
    {
        private readonly IODataAuthenticator _oDataAuthenticator;
        private readonly IODataHttpClient _oDataHttpClient;
        
        public AuthenticatedODataHttpClient(IODataAuthenticator oDataAuthenticator, IODataHttpClient oDataHttpClient)
        {
            _oDataAuthenticator = oDataAuthenticator;
            _oDataHttpClient = oDataHttpClient;
        }
        
        public async Task<HttpResponseMessage> SendAsync(
            Func<Task<HttpRequestMessage>> requestFactory,
            ODataHttpRequestOptions? options = null,
            CancellationToken cancellationToken = default
        )
        {
            var httpResponse = await _oDataHttpClient.SendAsync(async () =>
            {
                var httpRequest = await requestFactory();
                await _oDataAuthenticator.AddAuthenticationAsync(httpRequest, cancellationToken);
                return httpRequest;
            }, options, cancellationToken);

            return httpResponse;
        }
    }
}