using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OData.Client.Json.Net
{
    internal static class TypeExtensions
    {
        public static TValue? CreateEntityId<TValue>(this Type entityType, Guid value)
        {
            var entityIdType = typeof(EntityId<>).MakeGenericType(entityType);
            
            var instance = Activator.CreateInstance(entityIdType, value);
            if (instance is null)
            {
                return default;
            }

            return (TValue) instance;
        }
        
        public static bool IsEntityId(this Type type, [MaybeNullWhen(false)] out Type entityType)
        {
            if (type.IsInterface && type.IsEntityIdInterface(out entityType))
            {
                return true;
            }

            if (type.IsClass && type.IsEntityIdClass(out entityType))
            {
                return true;
            }

            entityType = null;
            return false;
        }

        private static bool IsEntityIdClass(this Type type, [MaybeNullWhen(false)] out Type entityType)
        {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(EntityId<>))
            {
                entityType = type.GetGenericArguments().Single();
                return true;
            }

            entityType = null;
            return false;
        }

        private static bool IsEntityIdInterface(this Type type, [MaybeNullWhen(false)] out Type entityType)
        {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(IEntityId<>))
            {
                entityType = type.GetGenericArguments().Single();
                return true;
            }

            entityType = null;
            return false;
        }
    }
}