using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Typed tolerant nullable enum converter.
/// </summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
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
