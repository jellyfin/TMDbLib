using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace TMDbLib.Utilities;

internal static class EnumMemberCache
{
    // Enum fields are always preserved by the runtime - the trimmer keeps every declared
    // static field of an enum because enums are value types - so DeclaredMembers reflection
    // is safe even with trimming.
    private const DynamicallyAccessedMemberTypes EnumMembers =
        DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields;

    private static readonly Dictionary<Type, Dictionary<object, string?>> _memberCache = [];

    private static Dictionary<object, string?> GetOrPrepareCache(
        [DynamicallyAccessedMembers(EnumMembers)] Type type)
    {
        if (!type.GetTypeInfo().IsEnum)
        {
            throw new ArgumentException();
        }

        Dictionary<object, string?>? cache;
        lock (_memberCache)
        {
            if (_memberCache.TryGetValue(type, out cache))
            {
                return cache;
            }
        }

        cache = [];

        foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            var value = fieldInfo.GetValue(null);
            if (value is null)
            {
                continue;
            }

            var attrib = fieldInfo.CustomAttributes.FirstOrDefault(s => s.AttributeType == typeof(EnumValueAttribute));

            if (attrib is null)
            {
                cache[value] = value.ToString();
            }
            else
            {
                var arg = attrib.ConstructorArguments.FirstOrDefault();
                var enumValue = arg.Value as string;

                cache[value] = enumValue;
            }
        }

        lock (_memberCache)
        {
            _memberCache[type] = cache;
        }

        return cache;
    }

    public static T? GetValue<[DynamicallyAccessedMembers(EnumMembers)] T>(string input)
    {
        var valueType = typeof(T);
        var cache = GetOrPrepareCache(valueType);

        foreach (var pair in cache)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(pair.Value, input))
            {
                return (T)pair.Key;
            }
        }

        return default;
    }

    public static object? GetValue(string? input, [DynamicallyAccessedMembers(EnumMembers)] Type type)
    {
        var cache = GetOrPrepareCache(type);

        foreach (var pair in cache)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(pair.Value, input))
            {
                return pair.Key;
            }
        }

        return null;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072:DynamicallyAccessedMembers", Justification = "Enum runtime types are preserved by the runtime - every declared static field is kept for value-type metadata.")]
    public static string? GetString(object? value)
    {
        if (value is null)
        {
            return null;
        }

        var valueType = value.GetType();
        var cache = GetOrPrepareCache(valueType);

        cache.TryGetValue(value, out var str);

        return str;
    }
}
