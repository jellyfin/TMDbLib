using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Reads a tagged image and resolves the polymorphic <c>media</c> sub-object based on
/// the outer <c>media_type</c>.
/// </summary>
internal class TaggedImageConverter : JsonConverter<TaggedImage>
{
    public override TaggedImage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

        // Pop the media node off so the default deserializer doesn't try to bind it
        // to the (no-converter) TmdbEntity? property — we'll re-attach the concrete object below.
        var mediaNode = node["media"];
        node.Remove("media");

        var deserOptions = WithoutThisConverter(options);
        var result = node.Deserialize<TaggedImage>(deserOptions);
        if (result is null)
        {
            return null;
        }

        if (mediaNode is not null)
        {
            Type targetType = result.MediaType switch
            {
                MediaType.Movie => typeof(SearchMovie),
                MediaType.Tv => typeof(SearchTv),
                MediaType.Episode => typeof(SearchTvEpisode),
                MediaType.Season => typeof(SearchTvSeason),
                _ => throw new ArgumentOutOfRangeException(nameof(reader), result.MediaType, "Unsupported tagged-image media type"),
            };

            result.Media = (TmdbEntity?)mediaNode.Deserialize(targetType, deserOptions);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, TaggedImage value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), WithoutThisConverter(options));
    }

    private static JsonSerializerOptions WithoutThisConverter(JsonSerializerOptions options)
    {
        var copy = new JsonSerializerOptions(options);
        for (var i = copy.Converters.Count - 1; i >= 0; i--)
        {
            if (copy.Converters[i] is TaggedImageConverter)
            {
                copy.Converters.RemoveAt(i);
            }
        }

        return copy;
    }
}
