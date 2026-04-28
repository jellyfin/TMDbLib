using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for UTC datetime values in TMDb's specific format.
/// </summary>
internal class TmdbUtcTimeConverterFactory : JsonConverterFactory
{
    private readonly TmdbUtcTimeConverter _tmdbUtcTimeConverter = new();
    private readonly TmdbUtcNullableTimeConverter _tmdbUtcNullableTimeConverter = new();
    private const string Format = "yyyy-MM-dd HH:mm:ss 'UTC'";

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) ||
               typeToConvert == typeof(DateTime?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(DateTime))
        {
            return _tmdbUtcTimeConverter;
        }

        if (typeToConvert == typeof(DateTime?))
        {
            return _tmdbUtcNullableTimeConverter;
        }

        return null;
    }

    private sealed class TmdbUtcTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                throw new JsonException("Expected a non-empty string for DateTime value.");
            }

            return DateTime.ParseExact(stringValue, Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }

    private sealed class TmdbUtcNullableTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            return DateTime.ParseExact(stringValue, Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}
