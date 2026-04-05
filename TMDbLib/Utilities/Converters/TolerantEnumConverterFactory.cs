using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for enum values that gracefully handles unrecognized values by falling back to defaults.
/// </summary>
public class TolerantEnumConverterFactory<TEnum> : JsonConverterFactory
    where TEnum : struct, Enum
{
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="typeToConvert">Type of the object.</param>
    /// <returns>True if this converter can convert the type; otherwise, false.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        var type = IsNullableType(typeToConvert) ? Nullable.GetUnderlyingType(typeToConvert) : typeToConvert;
        return type is not null && type == typeof(TEnum);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (IsNullableType(typeToConvert))
        {
            return new TolerantNullableEnumConverter();
        }

        return new TolerantEnumConverter();
    }

    private static bool IsNullableType(Type t)
    {
        return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    private static bool IsMatch(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        if (a.Equals(b, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        int aIndex;
        int bIndex;
        for (aIndex = 0, bIndex = 0; aIndex < a.Length && bIndex < b.Length; aIndex++, bIndex++)
        {
            if (a[aIndex].Equals(b[bIndex]))
            {
                continue;
            }

            if (char.ToLowerInvariant(a[aIndex]).Equals(char.ToLowerInvariant(b[bIndex])))
            {
                continue;
            }

            if (a[aIndex].Equals('_') && aIndex + 1 < a.Length && char.ToLowerInvariant(a[aIndex + 1]) == char.ToLowerInvariant(b[bIndex]))
            {
                aIndex++;
                continue;
            }

            if (b[bIndex].Equals('_') && bIndex + 1 < b.Length && char.ToLowerInvariant(b[bIndex + 1]) == char.ToLowerInvariant(a[aIndex]))
            {
                bIndex++;
                continue;
            }

            return false;
        }

        if (aIndex >= a.Length && bIndex >= b.Length)
        {
            return true;
        }

        return false;
    }

    private sealed class TolerantEnumConverter : JsonConverter<TEnum>
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var names = Enum.GetNames<TEnum>();

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var intValue))
            {
                if (Enum.IsDefined(typeof(TEnum), intValue))
                {
                    return Unsafe.BitCast<int, TEnum>(intValue);
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var enumText = reader.GetString();

                if (!string.IsNullOrEmpty(enumText))
                {
                    var match = names.FirstOrDefault(n => IsMatch(enumText, n));

                    if (match is not null)
                    {
                        return Enum.Parse<TEnum>(match);
                    }
                }
            }

            var defaultName = names.FirstOrDefault(n => IsMatch(n, "Unknown")) ?? names.First();
            return Enum.Parse<TEnum>(defaultName);
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    private sealed class TolerantNullableEnumConverter : JsonConverter<TEnum?>
    {
        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetInt32(out var intValue))
            {
                if (Enum.IsDefined(typeof(TEnum), intValue))
                {
                    return Unsafe.BitCast<int, TEnum>(intValue);
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var enumText = reader.GetString();

                if (!string.IsNullOrEmpty(enumText))
                {
                    var names = Enum.GetNames<TEnum>();
                    var match = names.FirstOrDefault(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase));

                    if (match is not null)
                    {
                        return Enum.Parse<TEnum>(match);
                    }
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.ToString());
        }
    }
}
