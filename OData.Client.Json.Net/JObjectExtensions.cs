using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal static class JObjectExtensions
    {
        public static T? GetValueOrDefault<T>(this JObject obj, string propertyName)
            where T : notnull
        {
            if (!obj.TryGetValue(propertyName, out var token))
            {
                return default;
            }

            return token.Value<T>();
        }

        public static T GetValue<T>(this JObject obj, string propertyName)
        {
            if (!obj.TryGetValue(propertyName, out var token))
            {
                throw new JsonSerializationException($"Expected a '{propertyName}' property in the response, but none was found.");
            }

            return token.Value<T>();
        }
    }
}