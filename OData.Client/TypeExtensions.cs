using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OData.Client
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Determines if <paramref name="type"/> is an enumerable type, and if so, returns the generic type argument
        /// of the enumerable type.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="valueType">The type of values produced by the enumerable type.</param>
        /// <returns><see langword="true"/> if the type is an enumerable type; otherwise, <see langword="false"/>.</returns>
        public static bool IsEnumerableType(this Type type, [MaybeNullWhen(false)] out Type valueType)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!@interface.IsConstructedGenericType)
                    continue;

                if (@interface.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                    continue;

                valueType = @interface.GetGenericArguments().Single();
                return true;
            }

            valueType = default;
            return false;
        }
    }
}