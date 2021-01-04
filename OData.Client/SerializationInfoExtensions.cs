using System;
using System.Runtime.Serialization;

namespace OData.Client
{
    internal static class SerializationInfoExtensions
    {
        public static void AddProperty(this SerializationInfo info, string name, IProperty property)
        {
            info.AddValue(name, new SerializableProperty(property));
        }

        public static IProperty GetNonNullableProperty(this SerializationInfo info, string name)
        {
            return info.GetNonNullableValue<SerializableProperty>(name);
        }

        private static T GetNonNullableValue<T>(this SerializationInfo info, string name)
            where T : notnull
        {
            var value = info.GetValue(name, typeof(T));
            if (value == null)
            {
                throw new SerializationException($"The value of '{name}' was null.");
            }

            return (T) value;
        }
        
        public static string GetNonNullableString(this SerializationInfo info, string name)
        {
            var value = info.GetString(name);
            if (value == null)
            {
                throw new SerializationException($"The value of '{name}' was null.");
            }
            
            return value;
        }
        
        public static Type GetNonNullableType(this SerializationInfo info, string name)
        {
            var typeName = info.GetNonNullableString(name);
            
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new SerializationException($"The type '{typeName}' could not be found.");
            }

            return type;
        }
    }
}