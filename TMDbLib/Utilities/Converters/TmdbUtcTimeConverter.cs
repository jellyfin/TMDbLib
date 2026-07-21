using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for UTC datetime values in TMDb's specific format.
/// </summary>
public class TmdbUtcTimeConverter : DateTimeConverterBase
{
    private const string Format = "yyyy-MM-dd HH:mm:ss 'UTC'";

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var stringValue = reader.Value?.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            return null;
        }

        return DateTime.ParseExact(stringValue, Format, null);
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            writer.WriteValue(dateTime.ToString(Format, CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteNull();
        }
    }
}
