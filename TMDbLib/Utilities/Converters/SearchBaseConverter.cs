using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Converters;

internal class SearchBaseConverter : JsonConverter<SearchBase?>
{
    public override SearchBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty("media_type", out JsonElement mediaTypeJson))
        {
            return (SearchBase?)jsonDocument.Deserialize(typeToConvert, TmdbJsonSerializerContext.Default);
        }

        var mediaType = mediaTypeJson.Deserialize(TmdbJsonSerializerContext.Default.MediaType);
        SearchBase? result = GetSearchBaseForMediaType(jsonDocument, mediaType, typeToConvert);
        return result ?? throw new JsonException("Unable to deserialize search base.");
    }

    public override void Write(Utf8JsonWriter writer, SearchBase? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), TmdbJsonSerializerContext.Default);
    }

    private static SearchBase? GetSearchBaseForMediaType(JsonDocument jsonDocument, MediaType mediaType, Type typeToConvert)
    {
        Type type = mediaType switch
        {
            MediaType.Movie => typeof(SearchMovie),
            MediaType.Tv => typeof(SearchTv),
            MediaType.Person => typeof(SearchPerson),
            MediaType.Episode => typeof(SearchTvEpisode),
            MediaType.TvEpisode => typeof(SearchTvEpisode),
            MediaType.Season => typeof(SearchTvSeason),
            MediaType.TvSeason => typeof(SearchTvSeason),
            MediaType.Collection => typeof(SearchCollection),
            _ => throw new ArgumentException()
        };

        if (type.IsAssignableFrom(typeToConvert))
        {
            return jsonDocument.Deserialize(typeToConvert, TmdbJsonSerializerContext.Default) as SearchBase;
        }

        return jsonDocument.Deserialize(type, TmdbJsonSerializerContext.Default) as SearchBase;
    }
}
