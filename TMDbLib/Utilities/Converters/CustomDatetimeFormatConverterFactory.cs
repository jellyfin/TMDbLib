using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for DateTime values with custom format strings.
/// </summary>
internal class CustomDatetimeFormatConverterFactory : JsonConverterFactory
{
    private readonly CustomDatetimeFormatConverter _customDatetimeFormatConverter = new();

    private readonly CustomNullableDatetimeFormatConverter _customNullableDatetimeFormatConverter = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomDatetimeFormatConverterFactory"/> class.
    /// </summary>
    public CustomDatetimeFormatConverterFactory()
    {
        CultureInfo = new CultureInfo("en-US");
        DatetimeFormat = "yyyy-MM-dd HH:mm:ss UTC";
        DateTimeStyles = DateTimeStyles.None;
    }

    /// <summary>
    /// Gets or sets the date time styles used when converting a date to and from JSON.
    /// </summary>
    public DateTimeStyles DateTimeStyles
    {
        get;
        set
        {
            field = value;
            UpdateConverter();
        }
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

    /// <summary>
    /// Determines whether this factory can create a converter for the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type to check.</param>
    /// <returns>
    /// <see langword="true"/> when <paramref name="typeToConvert"/> is
    /// <see cref="DateTime"/> or <see cref="Nullable{T}"/> of <see cref="DateTime"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);
    }

    /// <summary>
    /// Creates a JSON converter for supported date/time types.
    /// </summary>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options to use.</param>
    /// <returns>
    /// A <see cref="JsonConverter"/> for <see cref="DateTime"/> or <see cref="Nullable{DateTime}"/>.
    /// Returns <see langword="null"/> when the type is not supported.
    /// </returns>
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
        _customDatetimeFormatConverter.DatetimeStyles = DateTimeStyles;

        _customNullableDatetimeFormatConverter.CultureInfo = CultureInfo;
        _customNullableDatetimeFormatConverter.DatetimeFormat = DatetimeFormat;
        _customNullableDatetimeFormatConverter.DatetimeStyles = DateTimeStyles;
    }

    private sealed class CustomDatetimeFormatConverter : DateTimeConverterBase<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                throw new JsonException("Cannot convert null value to 'DateTime'.");
            }

            if (string.IsNullOrEmpty(DatetimeFormat))
            {
                return DateTime.Parse(stringValue, CultureInfo, DatetimeStyles);
            }

            return DateTime.ParseExact(stringValue, DatetimeFormat, CultureInfo, DatetimeStyles);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DatetimeFormat, CultureInfo));
        }
    }

    private sealed class CustomNullableDatetimeFormatConverter : DateTimeConverterBase<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            if (string.IsNullOrEmpty(DatetimeFormat))
            {
                return DateTime.Parse(stringValue, CultureInfo, DatetimeStyles);
            }

            return DateTime.ParseExact(stringValue, DatetimeFormat, CultureInfo, DatetimeStyles);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(DatetimeFormat, CultureInfo));
        }
    }
}
