using System;
using System.Net.Http;

namespace OData.Client
{
    public sealed class RateLimitPolicyContext
    {
        public RateLimitPolicyContext(HttpRequestMessage request, HttpResponseMessage response, DateTime retryAt, int attempts)
        {
            Request = request;
            Response = response;
            RetryAt = retryAt;
            Attempts = attempts;
        }

        public HttpRequestMessage Request { get; }
        public HttpResponseMessage Response { get; }
        public DateTime RetryAt { get; }
        public int Attempts { get; }
    }
}