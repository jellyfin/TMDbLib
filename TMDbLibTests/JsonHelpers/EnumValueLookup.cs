using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMDbLib.Utilities;

namespace TMDbLibTests.JsonHelpers;

/// <summary>
/// Reflection-based enum &lt;-&gt; string mapping, used by the snapshot serializer and by the tests
/// that verify the source-generated maps.
/// </summary>
/// <remarks>
/// TMDbLib itself no longer contains anything like this - its maps are source-generated so the
/// library stays reflection-free for native AOT. This deliberately independent implementation
/// reads <see cref="EnumValueAttribute"/> at runtime, so <c>SourceGeneratedEnumMapTests</c> can
/// use it to prove the generated switches agree with the attributes.
/// </remarks>
public static class EnumValueLookup
{
    private static readonly Dictionary<Type, Dictionary<object, string?>> _cache = [];

    /// <summary>
    /// Gets the mapped string for an enum value: the <see cref="EnumValueAttribute"/> value when
    /// declared, otherwise the member name; <c>null</c> for an undeclared value.
    /// </summary>
    /// <param name="value">The boxed enum value.</param>
    /// <returns>The mapped string, or <c>null</c>.</returns>
    public static string? GetString(object? value)
    {
        if (value is null)
        {
            return null;
        }

        GetOrBuild(value.GetType()).TryGetValue(value, out var str);

        return str;
    }

    /// <summary>
    /// Gets the enum value whose mapped string matches <paramref name="input"/>, ignoring case.
    /// </summary>
    /// <param name="input">The string to look up.</param>
    /// <param name="type">The enum type.</param>
    /// <returns>The matching boxed enum value, or <c>null</c>.</returns>
    public static object? GetValue(string? input, Type type)
    {
        foreach (var pair in GetOrBuild(type))
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(pair.Value, input))
            {
                return pair.Key;
            }
        }

        return null;
    }

    private static Dictionary<object, string?> GetOrBuild(Type type)
    {
        if (!type.IsEnum)
        {
            throw new ArgumentException($"{type} is not an enum", nameof(type));
        }

        lock (_cache)
        {
            if (_cache.TryGetValue(type, out var cached))
            {
                return cached;
            }
        }

        var map = new Dictionary<object, string?>();

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            var value = field.GetValue(null);
            if (value is null)
            {
                continue;
            }

            var attribute = field.CustomAttributes
                .FirstOrDefault(a => a.AttributeType == typeof(EnumValueAttribute));

            map[value] = attribute is null
                ? value.ToString()
                : attribute.ConstructorArguments.FirstOrDefault().Value as string;
        }

        lock (_cache)
        {
            _cache[type] = map;
        }

        return map;
    }
}
