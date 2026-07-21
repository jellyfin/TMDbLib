using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for partial or incomplete date values that may not parse correctly.
/// </summary>
public class TmdbPartialDateConverter : JsonConverter<DateTime?>
{
    /// <inheritdoc />
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var str = reader.GetString();
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        if (!DateTime.TryParse(str, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var result))
        {
            return null;
        }

        return result;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value.ToString(CultureInfo.InvariantCulture));
    }
}
