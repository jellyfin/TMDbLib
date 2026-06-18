using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Property-level converter that turns a JSON array of <c>known_for</c> items into a
/// <see cref="List{TmdbMediaSummary}"/> with each item polymorphically deserialised
/// to <see cref="TmdbMovieSummary"/> or <see cref="TmdbTvSummary"/> based on
/// the <c>media_type</c> discriminator.
/// </summary>
internal class KnownForConverter : JsonConverter<List<TmdbMediaSummary>?>
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
                MediaType.Movie => typeof(TmdbMovieSummary),
                MediaType.Tv => typeof(TmdbTvSummary),
                _ => throw new ArgumentOutOfRangeException(nameof(reader), mt.GetString(), "Unsupported known-for media type"),
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
