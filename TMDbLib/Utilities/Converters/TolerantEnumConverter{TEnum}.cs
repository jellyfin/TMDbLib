using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Typed JSON converter for enums that honours <see cref="EnumValueAttribute"/> mappings
/// and falls back gracefully on unrecognised values. Designed to be applied per-enum via
/// <c>[JsonConverter(typeof(TolerantEnumConverter&lt;MyEnum&gt;))]</c>; there is intentionally
/// no factory variant to keep the converter AOT-friendly (no runtime <c>MakeGenericType</c>).
/// </summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
public class TolerantEnumConverter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumText = reader.GetString();
            if (!string.IsNullOrEmpty(enumText))
            {
                // Honour EnumValue attribute mapping first (TMDb sends lowercase forms).
                var mapped = EnumMemberCache.GetValue<TEnum>(enumText);
                if (!Equals(mapped, default(TEnum)))
                {
                    return mapped;
                }

                if (Enum.TryParse<TEnum>(enumText, ignoreCase: true, out var parsed))
                {
                    return parsed;
                }
            }
        }
        else if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var enumVal))
        {
            var candidate = (TEnum)Enum.ToObject(typeof(TEnum), enumVal);
            if (Enum.IsDefined(candidate))
            {
                return candidate;
            }
        }

        // Fall through: pick the "Unknown" member if defined, otherwise the first declared value.
        var names = Enum.GetNames<TEnum>();
        var defaultName = names.FirstOrDefault(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase)) ?? names.First();
        return Enum.Parse<TEnum>(defaultName);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var str = EnumMemberCache.GetString(value);
        writer.WriteStringValue(str ?? value.ToString());
    }
}
