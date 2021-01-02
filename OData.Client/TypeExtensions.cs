using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OData.Client
{
    internal static class TypeExtensions
    {
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