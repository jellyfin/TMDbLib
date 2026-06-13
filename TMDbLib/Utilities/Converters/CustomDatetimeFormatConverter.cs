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

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var stringValue = reader.Value?.ToString();
        if (string.IsNullOrEmpty(stringValue))
        {
            return null;
        }

        return DateTime.ParseExact(stringValue, DatetimeFormat, CultureInfo.CurrentCulture);
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            writer.WriteValue(dateTime.ToString(DatetimeFormat, CultureInfo));
        }
        else
        {
            writer.WriteNull();
        }
    }
}
