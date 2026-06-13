using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter factory for DateTime values with custom format strings.
/// Supports both <see cref="DateTime"/> and <see cref="Nullable{DateTime}"/>.
/// </summary>
public class CustomDatetimeFormatConverter : JsonConverterFactory
{
    /// <summary>
    /// Gets or sets the culture info to use for date formatting.
    /// </summary>
    public CultureInfo CultureInfo { get; set; } = new CultureInfo("en-US");

    /// <summary>
    /// Gets or sets the datetime format string.
    /// </summary>
    public string DatetimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss UTC";

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(DateTime?))
        {
            return new NullableConverter(DatetimeFormat, CultureInfo);
        }

        return new Converter(DatetimeFormat, CultureInfo);
    }

    private sealed class Converter : JsonConverter<DateTime>
    {
        private readonly string _format;
        private readonly CultureInfo _culture;

        public Converter(string format, CultureInfo culture)
        {
            _format = format;
            _culture = culture;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return string.IsNullOrEmpty(stringValue)
                ? default
                : DateTime.ParseExact(stringValue, _format, _culture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format, _culture));
        }
    }

    private sealed class NullableConverter : JsonConverter<DateTime?>
    {
        private readonly string _format;
        private readonly CultureInfo _culture;

        public NullableConverter(string format, CultureInfo culture)
        {
            _format = format;
            _culture = culture;
        }

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            return DateTime.ParseExact(stringValue, _format, _culture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value.ToString(_format, _culture));
        }
    }
}
