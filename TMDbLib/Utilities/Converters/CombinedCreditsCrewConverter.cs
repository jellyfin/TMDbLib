using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.People;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Property-level converter that dispatches each item in the combined-credits crew
/// list to <see cref="CombinedCreditsCrewMovie"/> or <see cref="CombinedCreditsCrewTv"/>
/// based on the <c>media_type</c> discriminator.
/// </summary>
internal class CombinedCreditsCrewConverter : JsonConverter<List<TmdbMediaSummary>?>
{
    public override List<TmdbMediaSummary>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var list = new List<TmdbMediaSummary>();

        foreach (var element in document.RootElement.EnumerateArray())
        {
            if (!element.TryGetProperty("media_type", out var mt))
            {
                continue;
            }

            Type target = mt.Deserialize<MediaType>() switch
            {
                MediaType.Movie => typeof(CombinedCreditsCrewMovie),
                MediaType.Tv => typeof(CombinedCreditsCrewTv),
                _ => throw new ArgumentOutOfRangeException(nameof(reader), mt.GetString(), "Unsupported crew credit media type"),
            };

            var item = (TmdbMediaSummary?)element.Deserialize(target, options);
            if (item is not null)
            {
                list.Add(item);
            }
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<TmdbMediaSummary>? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, item.GetType(), options);
        }

        writer.WriteEndArray();
    }
}
