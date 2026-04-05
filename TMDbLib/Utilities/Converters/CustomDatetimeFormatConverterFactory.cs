using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for DateTime values with custom format strings.
/// </summary>
public class CustomDatetimeFormatConverterFactory : JsonConverterFactory
{
    private CustomDatetimeFormatConverter _customDatetimeFormatConverter = new();

    private CustomNullableDatetimeFormatConverter _customNullableDatetimeFormatConverter = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomDatetimeFormatConverter"/> class.
    /// </summary>
    public CustomDatetimeFormatConverterFactory()
    {
        CultureInfo = new CultureInfo("en-US");
        DatetimeFormat = "yyyy-MM-dd HH:mm:ss UTC";
    }

    /// <summary>
    /// Gets or sets the culture info to use for date formatting.
    /// </summary>
    public CultureInfo CultureInfo
    {
        get;
        set
        {
            field = value;
            UpdateConverter();
        }
    }

    /// <summary>
    /// Gets or sets the datetime format string.
    /// </summary>
    public string DatetimeFormat
    {
        get;
        set
        {
            field = value;
            UpdateConverter();
        }
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(DateTime))
        {
            return _customDatetimeFormatConverter;
        }

        if (typeToConvert == typeof(DateTime?))
        {
            return _customNullableDatetimeFormatConverter;
        }

        return null;
    }

    private void UpdateConverter()
    {
        _customDatetimeFormatConverter.CultureInfo = CultureInfo;
        _customDatetimeFormatConverter.DatetimeFormat = DatetimeFormat;

        _customNullableDatetimeFormatConverter.CultureInfo = CultureInfo;
        _customNullableDatetimeFormatConverter.DatetimeFormat = DatetimeFormat;
    }

    private sealed class CustomDatetimeFormatConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Gets or sets the culture info to use for date formatting.
        /// </summary>
        public CultureInfo CultureInfo { get; set; }

        /// <summary>
        /// Gets or sets the datetime format string.
        /// </summary>
        public string DatetimeFormat { get; set; }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                throw new JsonException("Cannot convert null value to 'DateTime'.");
            }

            return DateTime.ParseExact(stringValue, DatetimeFormat, CultureInfo);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DatetimeFormat, CultureInfo));
        }
    }

    private sealed class CustomNullableDatetimeFormatConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// Gets or sets the culture info to use for date formatting.
        /// </summary>
        public CultureInfo CultureInfo { get; set; }

        /// <summary>
        /// Gets or sets the datetime format string.
        /// </summary>
        public string DatetimeFormat { get; set; }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            return DateTime.ParseExact(stringValue, DatetimeFormat, CultureInfo);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(DatetimeFormat, CultureInfo));
        }
    }
}
