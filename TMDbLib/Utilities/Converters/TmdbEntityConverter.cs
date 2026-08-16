using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Polymorphic converter that maps the <c>media_type</c> discriminator to the correct
/// concrete <see cref="TmdbEntity"/> subclass. AOT-friendly: dispatches via the
/// configured <see cref="JsonSerializerOptions"/> source-generated metadata.
/// </summary>
internal class TmdbEntityConverter : JsonConverter<TmdbEntity>
{
    public override TmdbEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        if (!element.TryGetProperty("media_type", out var mediaTypeElement))
        {
            // Discriminator missing - empty entity (callers can't tell which concrete
            // subtype to materialize). Returning null keeps the call path AOT-safe.
            return null;
        }

        var mediaType = mediaTypeElement.Deserialize((JsonTypeInfo<MediaType>)options.GetTypeInfo(typeof(MediaType)));
        return mediaType switch
        {
            MediaType.Movie => (TmdbEntity?)element.Deserialize(options.GetTypeInfo(typeof(SearchMovie))),
            MediaType.Tv => (TmdbEntity?)element.Deserialize(options.GetTypeInfo(typeof(SearchTv))),
            MediaType.Person => (TmdbEntity?)element.Deserialize(options.GetTypeInfo(typeof(SearchPerson))),
            MediaType.Episode or MediaType.TvEpisode => (TmdbEntity?)element.Deserialize(options.GetTypeInfo(typeof(SearchTvEpisode))),
            MediaType.Season or MediaType.TvSeason => (TmdbEntity?)element.Deserialize(options.GetTypeInfo(typeof(SearchTvSeason))),
            MediaType.Collection => (TmdbEntity?)element.Deserialize(options.GetTypeInfo(typeof(SearchCollection))),
            _ => throw new ArgumentOutOfRangeException(nameof(reader), mediaType, "Unsupported media type"),
        };
    }

    public override void Write(Utf8JsonWriter writer, TmdbEntity value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var typeInfo = options.GetTypeInfo(value.GetType());
        JsonSerializer.Serialize(writer, value, typeInfo);
    }
}
