using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Polymorphic converter that maps the <c>media_type</c> discriminator to the correct
/// concrete <see cref="TmdbEntity"/> subclass for endpoints returning mixed lists
/// (search/multi, trending/all, list items, tagged_images.media, etc.).
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

        Type targetType;
        if (!element.TryGetProperty("media_type", out var mediaTypeElement))
        {
            // Discriminator missing — fall back to the requested concrete type.
            return (TmdbEntity?)Activator.CreateInstance(typeToConvert);
        }

        var mediaType = mediaTypeElement.Deserialize<MediaType>();
        targetType = mediaType switch
        {
            MediaType.Movie => typeof(SearchMovie),
            MediaType.Tv => typeof(SearchTv),
            MediaType.Person => typeof(SearchPerson),
            MediaType.Episode => typeof(SearchTvEpisode),
            MediaType.TvEpisode => typeof(SearchTvEpisode),
            MediaType.Season => typeof(SearchTvSeason),
            MediaType.TvSeason => typeof(SearchTvSeason),
            MediaType.Collection => typeof(SearchCollection),
            _ => throw new ArgumentOutOfRangeException(nameof(reader), mediaType, "Unsupported media type"),
        };

        return (TmdbEntity?)element.Deserialize(targetType, options);
    }

    public override void Write(Utf8JsonWriter writer, TmdbEntity value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
