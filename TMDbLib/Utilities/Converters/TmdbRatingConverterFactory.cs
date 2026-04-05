using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

internal class TmdbRatingConverterFactory : JsonConverterFactory
{
    private readonly JsonConverter<double> _doubleConverter = new TmdbRatingConverter();

    private readonly JsonConverter<double?> _nullableDoubleConverter = new TmdbNullableRatingConverter();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(double) || typeToConvert == typeof(double?);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(double))
        {
            return _doubleConverter;
        }

        if (typeToConvert == typeof(double?))
        {
            return _nullableDoubleConverter;
        }

        return null;
    }

    private sealed class TmdbRatingConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.False)
            {
                throw new JsonException("Cannot convert false to a double rating.");
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                throw new JsonException("Cannot convert true to a double rating.");
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out double value))
                {
                    return value;
                }

                throw new JsonException("Invalid number format for double rating.");
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                if (doc.RootElement.TryGetProperty("value", out JsonElement valueElement))
                {
                    if (valueElement.ValueKind == JsonValueKind.Number)
                    {
                        return valueElement.GetDouble();
                    }
                }

                throw new JsonException("Invalid number format for double rating.");
            }

            throw new JsonException("Invalid number format for double rating.");
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("value", value);
            writer.WriteEndObject();
        }
    }

    private sealed class TmdbNullableRatingConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.False)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out double value))
                {
                    return value;
                }

                return null;
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                if (doc.RootElement.TryGetProperty("value", out JsonElement valueElement))
                {
                    if (valueElement.ValueKind == JsonValueKind.Number)
                    {
                        return valueElement.GetDouble();
                    }
                }

                return null;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteBooleanValue(false);
                return;
            }

            writer.WriteStartObject();
            writer.WriteNumber("value", value.Value);
            writer.WriteEndObject();
        }
    }
}
