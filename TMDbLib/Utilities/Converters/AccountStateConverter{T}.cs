using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Typed normaliser between the wire <c>rated</c> shape (either <c>false</c> or
/// <c>{ "value": N }</c>) and the C# <c>rating</c> property. Registered per-type
/// from <see cref="Serializer.TMDbJsonSerializer"/> with a source-generated
/// <see cref="JsonTypeInfo{T}"/> so dispatch stays AOT-friendly.
/// </summary>
/// <typeparam name="T">The concrete account-state type.</typeparam>
internal class AccountStateConverter<T> : JsonConverter<T>
    where T : class, new()
{
    private readonly JsonTypeInfo<T> _typeInfo;

    public AccountStateConverter(JsonTypeInfo<T> typeInfo)
    {
        _typeInfo = typeInfo;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var node = JsonNode.Parse(ref reader)?.AsObject();
        if (node is null)
        {
            return null;
        }

        // Normalise rated → rating
        //  "rated": false      → "rating": null
        //  "rated": {value:N}  → "rating": N
        if (node.TryGetPropertyValue("rated", out var ratedNode))
        {
            node.Remove("rated");

            if (ratedNode is JsonValue jv && jv.TryGetValue<bool>(out _))
            {
                node["rating"] = null;
            }
            else if (ratedNode is JsonObject jo && jo.TryGetPropertyValue("value", out var valueNode))
            {
                node["rating"] = valueNode?.GetValue<double>();
            }
        }

        return node.Deserialize(_typeInfo);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var node = JsonSerializer.SerializeToNode(value, _typeInfo)?.AsObject();
        if (node is null)
        {
            writer.WriteNullValue();
            return;
        }

        // Reverse: rating → rated
        if (node.TryGetPropertyValue("rating", out var ratingNode))
        {
            node.Remove("rating");

            if (ratingNode is null || (ratingNode is JsonValue jv && !jv.TryGetValue<double>(out _)))
            {
                node["rated"] = false;
            }
            else
            {
                node["rated"] = new JsonObject { ["value"] = ratingNode.DeepClone() };
            }
        }

        node.WriteTo(writer);
    }
}
