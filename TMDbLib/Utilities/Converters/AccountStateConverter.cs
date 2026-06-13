using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Normalises the TMDb <c>rated</c> field (which is either <c>false</c> or
/// <c>{ "value": n }</c>) into a <c>rating</c> property on the C# object.
/// </summary>
internal class AccountStateConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(AccountState)
            || typeToConvert == typeof(TvAccountState)
            || typeToConvert == typeof(TvEpisodeAccountState)
            || typeToConvert == typeof(TvEpisodeAccountStateWithNumber);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(AccountStateConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

/// <summary>
/// Typed normaliser between the wire <c>rated</c> shape and the C# <c>rating</c> property.
/// </summary>
internal class AccountStateConverter<T> : JsonConverter<T>
    where T : class, new()
{
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

        return node.Deserialize<T>(WithoutThisConverter(options));
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var node = JsonSerializer.SerializeToNode(value, value.GetType(), WithoutThisConverter(options))?.AsObject();
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

        node.WriteTo(writer, WithoutThisConverter(options));
    }

    private static JsonSerializerOptions WithoutThisConverter(JsonSerializerOptions options)
    {
        // Create a copy that excludes this converter to avoid recursion.
        var copy = new JsonSerializerOptions(options);
        for (var i = copy.Converters.Count - 1; i >= 0; i--)
        {
            if (copy.Converters[i] is AccountStateConverter)
            {
                copy.Converters.RemoveAt(i);
            }
        }

        return copy;
    }
}
