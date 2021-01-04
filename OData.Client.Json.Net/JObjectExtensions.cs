using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OData.Client.Json.Net
{
    internal static class JObjectExtensions
    {
        /// <summary>
        /// Gets the value with the specified <paramref name="propertyName"/> in <paramref name="obj"/>, using the
        /// <paramref name="serializer"/> to convert it to <typeparamref name="T"/>, throwing an exception if the value
        /// is <see langword="null"/>.
        /// </summary>
        /// <param name="obj">The JSON object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="serializer">The serializer.</param>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <returns>The instance of <typeparamref name="T"/> being deserialized to, or <see langword="null"/> if the
        /// property is not present.</returns>
        /// <exception cref="JsonSerializationException">The value could not be converted to <typeparamref name="T"/>,
        /// or the value was null.</exception>
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

        /// <summary>
        /// Gets the value with the specified <paramref name="propertyName"/> in <paramref name="obj"/>, using the
        /// <paramref name="serializer"/> to convert it to <typeparamref name="T"/>, throwing an exception if the value
        /// is <see langword="null"/> or the property is not present.
        /// </summary>
        /// <param name="obj">The JSON object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="serializer">The serializer.</param>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <returns>The instance of <typeparamref name="T"/> being deserialized to.</returns>
        /// <exception cref="JsonSerializationException">The value could not be converted to <typeparamref name="T"/>,
        /// the value was null, or the property was not present.</exception>
        public static T GetValue<T>(this JObject obj, string propertyName, JsonSerializer serializer)
            where T : notnull
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