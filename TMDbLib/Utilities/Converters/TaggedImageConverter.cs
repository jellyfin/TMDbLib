using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Reads a tagged image and resolves the polymorphic <c>media</c> sub-object based on
/// the outer <c>media_type</c>. Parses every field manually so the codepath is fully
/// AOT/trim-safe - no reflection-based fallback needed.
/// </summary>
internal class TaggedImageConverter : JsonConverter<TaggedImage>
{
    public override TaggedImage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        var result = new TaggedImage();

        if (element.TryGetProperty("aspect_ratio", out var aspectRatio))
        {
            result.AspectRatio = aspectRatio.GetDouble();
        }

        if (element.TryGetProperty("file_path", out var filePath))
        {
            result.FilePath = filePath.GetString();
        }

        if (element.TryGetProperty("height", out var height))
        {
            result.Height = height.GetInt32();
        }

        if (element.TryGetProperty("id", out var id))
        {
            result.Id = id.GetString();
        }

        if (element.TryGetProperty("image_type", out var imageType))
        {
            result.ImageType = imageType.GetString();
        }

        if (element.TryGetProperty("iso_639_1", out var iso))
        {
            result.Iso_639_1 = iso.GetString();
        }

        if (element.TryGetProperty("media_type", out var mediaTypeElement))
        {
            result.MediaType = mediaTypeElement.Deserialize((JsonTypeInfo<MediaType>)options.GetTypeInfo(typeof(MediaType)));
        }

        if (element.TryGetProperty("vote_average", out var voteAverage))
        {
            result.VoteAverage = voteAverage.GetDouble();
        }

        if (element.TryGetProperty("vote_count", out var voteCount))
        {
            result.VoteCount = voteCount.GetInt32();
        }

        if (element.TryGetProperty("width", out var width))
        {
            result.Width = width.GetInt32();
        }

        if (element.TryGetProperty("media", out var mediaElement) && mediaElement.ValueKind != JsonValueKind.Null)
        {
            result.Media = result.MediaType switch
            {
                MediaType.Movie => (TMDbLib.Objects.General.Schema.TmdbEntity?)mediaElement.Deserialize(options.GetTypeInfo(typeof(SearchMovie))),
                MediaType.Tv => (TMDbLib.Objects.General.Schema.TmdbEntity?)mediaElement.Deserialize(options.GetTypeInfo(typeof(SearchTv))),
                MediaType.Episode => (TMDbLib.Objects.General.Schema.TmdbEntity?)mediaElement.Deserialize(options.GetTypeInfo(typeof(SearchTvEpisode))),
                MediaType.Season => (TMDbLib.Objects.General.Schema.TmdbEntity?)mediaElement.Deserialize(options.GetTypeInfo(typeof(SearchTvSeason))),
                _ => throw new ArgumentOutOfRangeException(nameof(reader), result.MediaType, "Unsupported tagged-image media type"),
            };
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

        writer.WriteStartObject();
        writer.WriteNumber("aspect_ratio", value.AspectRatio);
        writer.WriteString("file_path", value.FilePath);
        writer.WriteNumber("height", value.Height);
        writer.WriteString("id", value.Id);
        writer.WriteString("image_type", value.ImageType);
        writer.WriteString("iso_639_1", value.Iso_639_1);

        writer.WritePropertyName("media_type");
        JsonSerializer.Serialize(writer, value.MediaType, options.GetTypeInfo(typeof(MediaType)));

        writer.WriteNumber("vote_average", value.VoteAverage);
        writer.WriteNumber("vote_count", value.VoteCount);
        writer.WriteNumber("width", value.Width);

        writer.WritePropertyName("media");
        if (value.Media is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            var typeInfo = options.GetTypeInfo(value.Media.GetType());
            JsonSerializer.Serialize(writer, value.Media, typeInfo);
        }

        writer.WriteEndObject();
    }
}
