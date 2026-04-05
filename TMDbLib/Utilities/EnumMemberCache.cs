using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace TMDbLib.Utilities;

internal static class EnumMemberCache
{
    private static readonly Dictionary<Type, Dictionary<object, string?>> _memberCache = [];

    private static Dictionary<object, string?> GetOrPrepareCache([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type)
    {
        if (!type.IsEnum)
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

        foreach (var fieldInfo in type.GetTypeInfo().GetFields().Where(s => s.IsStatic))
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

    private static Dictionary<object, string?> GetOrPrepareCache<T>()
    where T : struct, Enum
    {
        Type type = typeof(T);
        Dictionary<object, string?>? cache;
        lock (_memberCache)
        {
            if (_memberCache.TryGetValue(type, out cache))
            {
                return cache;
            }
        }

        cache = [];

        foreach (var fieldInfo in type.GetFields().Where(s => s.IsStatic))
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

    public static T? GetValue<T>(string input)
        where T : struct, Enum
    {
        var cache = GetOrPrepareCache<T>();

        foreach (var pair in cache)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(pair.Value, input))
            {
                return (T)pair.Key;
            }
        }

        return default;
    }

    public static object? GetValue(string? input, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type)
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

    public static string? GetString([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] Type type)
    {
        var cache = GetOrPrepareCache(type);
        cache.TryGetValue(type, out var str);
        return str;
    }
}
