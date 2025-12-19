using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for DateTime values with custom format strings.
/// </summary>
public class CustomDatetimeFormatConverter : DateTimeConverterBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomDatetimeFormatConverter"/> class.
    /// </summary>
    public CustomDatetimeFormatConverter()
    {
        CultureInfo = new CultureInfo("en-US");
        DatetimeFormat = "yyyy-MM-dd HH:mm:ss UTC";
    }

    /// <summary>
    /// Gets or sets the culture info to use for date formatting.
    /// </summary>
    public CultureInfo CultureInfo { get; set; }

    /// <summary>
    /// Gets or sets the datetime format string.
    /// </summary>
    public string DatetimeFormat { get; set; }

    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return DateTime.ParseExact(reader.Value.ToString(), DatetimeFormat, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(((DateTime)value).ToString(DatetimeFormat, CultureInfo));
    }
}
