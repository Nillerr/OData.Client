using System;

namespace OData.Client.Json.Net
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Determines if the type is either a <see cref="IEntityId{TEntity}"/> or <see cref="EntityId{TEntity}"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true"/> if the type is a constructed generic type of
        /// <see cref="IEntityId{TEntity}"/> or <see cref="IsEntityId"/>; otherwise <see langword="false"/>.</returns>
        public static bool IsEntityId(this Type type)
        {
            if (type.IsInterface && type.IsEntityIdInterface())
            {
                return true;
            }

            if (type.IsClass && type.IsEntityIdClass())
            {
                return true;
            }

            return false;
        }

        private static bool IsEntityIdClass(this Type type)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(EntityId<>);
        }

        private static bool IsEntityIdInterface(this Type type)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(IEntityId<>);
        }
    }
}