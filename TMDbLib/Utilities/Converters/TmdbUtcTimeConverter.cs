using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for UTC datetime values returned by TMDb. Accepts both
/// "yyyy-MM-dd HH:mm:ss UTC" (used intermittently on the videos and changes
/// endpoints) and ISO-8601, since TMDb has historically alternated between
/// the two for the same field. Always serializes in the "UTC"-suffixed form
/// to preserve existing on-the-wire behavior.
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
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // Newtonsoft's reader auto-parses ISO-8601 strings into DateTime when
        // DateParseHandling = DateTime (the default). Accept that as-is.
        if (reader.Value is DateTime dt)
        {
            return dt;
        }

        var stringValue = reader.Value?.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            return null;
        }

        const DateTimeStyles styles = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;

        // TMDb's "yyyy-MM-dd HH:mm:ss UTC" form.
        if (DateTime.TryParseExact(stringValue, Format, CultureInfo.InvariantCulture, styles, out var exact))
        {
            return exact;
        }

        // Fall back to ISO-8601 / any other parseable form so the converter
        // remains correct if TMDb reverts the format again.
        return DateTime.Parse(stringValue, CultureInfo.InvariantCulture, styles);
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="serializer">The calling serializer.</param>
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
