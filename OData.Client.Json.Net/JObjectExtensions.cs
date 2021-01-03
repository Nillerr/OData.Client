using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal static class JObjectExtensions
    {
        public static T? GetValueOrDefault<T>(this JObject obj, string propertyName, JsonSerializer serializer)
            where T : notnull
        {
            if (!obj.TryGetValue(propertyName, out var token))
            {
                return default;
            }

            using var reader = new JTokenReader(token);
            
            var value = serializer.Deserialize<T>(reader);
            if (value == null)
            {
                throw new JsonSerializationException($"Expected property '{propertyName}' to be non-null, but was null.");
            }
            
            return value;
        }

        public static T GetValue<T>(this JObject obj, string propertyName, JsonSerializer serializer)
        {
            if (!obj.TryGetValue(propertyName, out var token))
            {
                throw new JsonSerializationException($"Expected a '{propertyName}' property in the response, but none was found.");
            }

            using var reader = new JTokenReader(token);
            
            var value = serializer.Deserialize<T>(reader);
            if (value == null)
            {
                throw new JsonSerializationException($"Expected property '{propertyName}' to be non-null, but was null.");
            }
            
            return value;
        }
    }
}