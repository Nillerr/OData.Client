using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OData.Client
{
    internal static class HttpHeaderCollectionExtensions
    {
        /// <summary>
        /// Returns the value of the <c>Retry-After</c> header as a <see cref="DateTime"/> in UTC, using the provided
        /// <paramref name="currentInstant"/> when the header contains a delta in seconds..
        /// </summary>
        /// <param name="headers">The response headers.</param>
        /// <param name="currentInstant">The current time.</param>
        /// <returns>The time to retry the request after.</returns>
        /// <exception cref="HttpRequestException">The 'Retry-After' header either wasn't present, or was invalid.</exception>
        public static DateTime GetRetryAt(this HttpResponseHeaders headers, DateTime currentInstant)
        {
            var retryAfter = headers.RetryAfter;
            if (retryAfter == null)
            {
                throw new HttpRequestException("Expected the 'Retry-After' header to be present, but it wasn't.");
            }

            var delta = retryAfter.Delta;
            var date = retryAfter.Date;
            if (delta.HasValue)
            {
                return currentInstant + delta.Value;
            }

            if (date.HasValue)
            {
                return date.Value.UtcDateTime;
            }

            throw new HttpRequestException("Expected the 'Retry-After' header to contain a valid value.");
        }
    }
}