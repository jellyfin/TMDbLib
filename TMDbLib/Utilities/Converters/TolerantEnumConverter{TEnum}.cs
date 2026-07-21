using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Typed tolerant enum converter.
/// </summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
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
