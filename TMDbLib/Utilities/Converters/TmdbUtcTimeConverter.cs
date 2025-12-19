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

    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The parsed DateTime value.</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return DateTime.ParseExact(reader.Value.ToString(), Format, null);
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((DateTime)value).ToString(Format, CultureInfo.InvariantCulture));
    }
}
