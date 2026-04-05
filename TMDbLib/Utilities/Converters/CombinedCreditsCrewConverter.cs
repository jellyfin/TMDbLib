using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Utilities.JsonSerializerContexts;

namespace TMDbLib.Utilities.Converters;

internal class CombinedCreditsCrewConverter : JsonConverter<CombinedCreditsCrewBase>
{
    public override CombinedCreditsCrewBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        if (!document.RootElement.TryGetProperty("media_type", out JsonElement mediaTypeElement))
        {
            return null;
        }

        var mediaType = mediaTypeElement.Deserialize(TmdbJsonSerializerContext.Default.MediaType);
        return mediaType switch
        {
            MediaType.Movie => document.Deserialize(TmdbJsonSerializerContext.Default.CombinedCreditsCrewMovie),
            MediaType.Tv => document.Deserialize(TmdbJsonSerializerContext.Default.CombinedCreditsCrewTv),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Write(Utf8JsonWriter writer, CombinedCreditsCrewBase value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), TmdbJsonSerializerContext.Default);
    }
}
