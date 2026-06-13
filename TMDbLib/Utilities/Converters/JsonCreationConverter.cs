using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Polymorphic converter base — reads the JSON into a <see cref="JsonDocument"/>,
/// asks the derived class to pick the concrete subtype, then deserializes into it.
/// </summary>
/// <typeparam name="T">Polymorphic root type.</typeparam>
internal abstract class JsonCreationConverter<T> : JsonConverter<T>
    where T : class
{
    /// <summary>
    /// Returns the concrete subtype to deserialize into, based on the root JSON element.
    /// </summary>
    /// <returns></returns>
    protected abstract Type? GetTargetType(JsonElement element);

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        var targetType = GetTargetType(element);
        if (targetType is null)
        {
            return null;
        }

        return (T?)element.Deserialize(targetType, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
