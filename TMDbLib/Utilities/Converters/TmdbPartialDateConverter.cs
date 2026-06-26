using System;
using System.Globalization;
using Newtonsoft.Json;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for partial or incomplete date values that may not parse correctly.
/// </summary>
public class TmdbPartialDateConverter : JsonConverter
{
    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime?);
    }

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var str = reader.Value as string;
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
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var date = value as DateTime?;
        if (date is null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(date.Value.ToString(CultureInfo.InvariantCulture));
    }
}
