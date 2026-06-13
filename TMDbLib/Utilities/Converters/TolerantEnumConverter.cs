using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Converter factory for enums that gracefully handles unrecognized values by falling back to defaults.
/// </summary>
public class TolerantEnumConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        var type = IsNullableType(typeToConvert) ? Nullable.GetUnderlyingType(typeToConvert) : typeToConvert;
        return type is not null && type.GetTypeInfo().IsEnum;
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var isNullable = IsNullableType(typeToConvert);
        var enumType = isNullable ? Nullable.GetUnderlyingType(typeToConvert)! : typeToConvert;

        var converterType = isNullable
            ? typeof(TolerantNullableEnumConverter<>).MakeGenericType(enumType)
            : typeof(TolerantEnumConverter<>).MakeGenericType(enumType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private static bool IsNullableType(Type t)
    {
        return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}

/// <summary>
/// Typed tolerant enum converter.
/// </summary>
internal class TolerantEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var names = Enum.GetNames<TEnum>();

        if (reader.TokenType == JsonTokenType.String)
        {
            var enumText = reader.GetString();
            if (!string.IsNullOrEmpty(enumText))
            {
                // Honour EnumValue attribute mapping first (TMDb sends lowercase forms).
                var mapped = EnumMemberCache.GetValue(enumText, typeof(TEnum));
                if (mapped is TEnum mappedTyped)
                {
                    return mappedTyped;
                }

                var match = names.FirstOrDefault(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase));
                if (match is not null && Enum.TryParse<TEnum>(match, out var parsed))
                {
                    return parsed;
                }
            }
        }
        else if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var enumVal))
        {
            var values = (int[])(object)Enum.GetValues<TEnum>();
            if (values.Contains(enumVal))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), enumVal);
            }
        }

        var defaultName = names.FirstOrDefault(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase)) ?? names.First();
        return Enum.Parse<TEnum>(defaultName);
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var str = EnumMemberCache.GetString(value);
        writer.WriteStringValue(str ?? value.ToString());
    }
}

/// <summary>
/// Typed tolerant nullable enum converter.
/// </summary>
internal class TolerantNullableEnumConverter<TEnum> : JsonConverter<TEnum?>
    where TEnum : struct, Enum
{
    public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var names = Enum.GetNames<TEnum>();

        if (reader.TokenType == JsonTokenType.String)
        {
            var enumText = reader.GetString();
            if (!string.IsNullOrEmpty(enumText))
            {
                // Honour EnumValue attribute mapping first (TMDb sends lowercase forms).
                var mapped = EnumMemberCache.GetValue(enumText, typeof(TEnum));
                if (mapped is TEnum mappedTyped)
                {
                    return mappedTyped;
                }

                var match = names.FirstOrDefault(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase));
                if (match is not null && Enum.TryParse<TEnum>(match, out var parsed))
                {
                    return parsed;
                }
            }
        }
        else if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var enumVal))
        {
            var values = (int[])(object)Enum.GetValues<TEnum>();
            if (values.Contains(enumVal))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), enumVal);
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

        var str = EnumMemberCache.GetString(value.Value);
        writer.WriteStringValue(str ?? value.Value.ToString());
    }
}
