using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Typed enum converter that maps to/from the <c>EnumValue</c>-decorated string.
/// </summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
internal class EnumStringValueConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        var value = EnumMemberCache.GetValue(str, typeToConvert);
        return value is TEnum typed ? typed : default;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var str = EnumMemberCache.GetString(value);
        if (str is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(str);
    }
}
