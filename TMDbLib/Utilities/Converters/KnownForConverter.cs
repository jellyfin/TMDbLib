using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
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
        var mediaTypeInfo = (JsonTypeInfo<MediaType>)options.GetTypeInfo(typeof(MediaType));

        foreach (var element in document.RootElement.EnumerateArray())
        {
            if (!element.TryGetProperty("media_type", out var mt))
            {
                continue;
            }

            TmdbMediaSummary? item = mt.Deserialize(mediaTypeInfo) switch
            {
                MediaType.Movie => (TmdbMediaSummary?)element.Deserialize(options.GetTypeInfo(typeof(TmdbMovieSummary))),
                MediaType.Tv => (TmdbMediaSummary?)element.Deserialize(options.GetTypeInfo(typeof(TmdbTvSummary))),
                _ => throw new ArgumentOutOfRangeException(nameof(reader), mt.GetString(), "Unsupported known-for media type"),
            };

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
            if (item is null)
            {
                writer.WriteNullValue();
                continue;
            }

            var typeInfo = options.GetTypeInfo(item.GetType());
            JsonSerializer.Serialize(writer, item, typeInfo);
        }

        writer.WriteEndArray();
    }
}
