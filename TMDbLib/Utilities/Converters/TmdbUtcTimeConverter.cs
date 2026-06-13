using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter factory for UTC datetime values in TMDb's specific format
/// (<c>yyyy-MM-dd HH:mm:ss UTC</c>). Supports both <see cref="DateTime"/> and
/// <see cref="Nullable{DateTime}"/> properties.
/// </summary>
public class TmdbUtcTimeConverter : JsonConverterFactory
{
    private const string Format = "yyyy-MM-dd HH:mm:ss 'UTC'";

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
            return new NullableConverter();
        }

        return new Converter();
    }

    private sealed class Converter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return default;
            }

            return DateTime.ParseExact(stringValue, Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }

    private sealed class NullableConverter : JsonConverter<DateTime?>
    {
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

            return DateTime.ParseExact(stringValue, Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
