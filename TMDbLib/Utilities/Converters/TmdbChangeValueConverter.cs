using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// JSON converter for the loosely typed change-item values TMDb returns, which may be a
/// primitive or a nested object/array.
/// </summary>
/// <remarks>
/// System.Text.Json binds <see cref="object"/> to a <see cref="JsonElement"/>, whereas
/// Newtonsoft materialised primitives as CLR values. This restores the latter so consumers
/// can keep casting to <see cref="string"/>, <see cref="long"/>, <see cref="double"/> or
/// <see cref="bool"/>; objects and arrays are still surfaced as a <see cref="JsonElement"/>.
/// </remarks>
public class TmdbChangeValueConverter : JsonConverter<object?>
{
    /// <inheritdoc />
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.String:
                return reader.GetString();
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Number:
                return reader.TryGetInt64(out var integer) ? integer : reader.GetDouble();
            default:
                // Objects and arrays keep their JSON shape; the caller decides how to read them.
                using (var document = JsonDocument.ParseValue(ref reader))
                {
                    return document.RootElement.Clone();
                }
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        // Write the shapes Read can produce directly - no reflection, so the codepath
        // stays AOT/trim-safe and doesn't depend on the resolver knowing these types.
        switch (value)
        {
            case null:
                writer.WriteNullValue();
                return;
            case string text:
                writer.WriteStringValue(text);
                return;
            case bool boolean:
                writer.WriteBooleanValue(boolean);
                return;
            case long integer:
                writer.WriteNumberValue(integer);
                return;
            case double number:
                writer.WriteNumberValue(number);
                return;
            case JsonElement element:
                element.WriteTo(writer);
                return;
        }

        var runtimeType = value.GetType();

        // Guard against recursing back into this converter for a bare object instance.
        if (runtimeType == typeof(object))
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
            return;
        }

        JsonSerializer.Serialize(writer, value, options.GetTypeInfo(runtimeType));
    }
}
