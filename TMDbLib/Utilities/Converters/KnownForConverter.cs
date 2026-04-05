using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Converters;

internal class KnownForConverter : JsonConverter<KnownForBase>
{
    public override KnownForBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        if (!document.RootElement.TryGetProperty("media_type", out JsonElement mediaTypeElement))
        {
            return null;
        }

        var mediaType = mediaTypeElement.Deserialize(TmdbJsonSerializerContext.Default.MediaType);
        return mediaType switch
        {
            MediaType.Movie => document.Deserialize(TmdbJsonSerializerContext.Default.KnownForMovie),
            MediaType.Tv => document.Deserialize(TmdbJsonSerializerContext.Default.KnownForTv),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Write(Utf8JsonWriter writer, KnownForBase value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), TmdbJsonSerializerContext.Default);
    }
}
