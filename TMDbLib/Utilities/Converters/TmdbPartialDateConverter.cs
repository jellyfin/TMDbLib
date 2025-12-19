using System;
using System.Globalization;
using Newtonsoft.Json;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for partial or incomplete date values that may not parse correctly.
/// </summary>
public class TmdbPartialDateConverter : JsonConverter
{
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>True if this converter can convert the type; otherwise, false.</returns>
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime?);
    }

    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The parsed DateTime value, or null if parsing fails.</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string str = reader.Value as string;
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

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        DateTime? date = value as DateTime?;
        writer.WriteValue(date?.ToString(CultureInfo.InvariantCulture));
    }
}
