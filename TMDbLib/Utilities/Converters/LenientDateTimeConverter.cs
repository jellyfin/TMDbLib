using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Global converter for <see cref="DateTime"/> and <see cref="Nullable{DateTime}"/> that
/// tolerates the TMDb wire-format quirks: empty strings, unparseable values, and the
/// "yyyy-MM-dd" partial format alongside ISO 8601.
/// </summary>
internal class LenientDateTimeConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return typeToConvert == typeof(DateTime?) ? new NullableConverter() : new Converter();
    }

    private sealed class Converter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return dt;
                }

                return reader.TryGetDateTimeLenient(CultureInfo.InvariantCulture, out var parsed) ? parsed : default;
            }

            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
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

            if (reader.TokenType == JsonTokenType.String)
            {
                if (reader.TryGetDateTime(out var dt))
                {
                    return dt;
                }

                return reader.TryGetDateTimeLenient(CultureInfo.InvariantCulture, out var parsed) ? parsed : null;
            }

            if (reader.TryGetDateTime(out var nonStringDt))
            {
                return nonStringDt;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value);
        }
    }
}
