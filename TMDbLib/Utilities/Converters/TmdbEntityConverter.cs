using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

/// <summary>
/// Polymorphic converter that maps the <c>media_type</c> discriminator to the correct
/// concrete <see cref="TmdbEntity"/> subclass for endpoints returning mixed lists
/// (search/multi, trending/all, list items, tagged_images.media, etc.).
/// </summary>
internal class TmdbEntityConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TmdbEntity);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        TmdbEntity? result;
        if (jObject["media_type"] is null)
        {
            // Discriminator missing — fall back to the requested concrete type.
            result = Activator.CreateInstance(objectType) as TmdbEntity;
        }
        else
        {
            var mediaType = jObject["media_type"]!.ToObject<MediaType>();

            result = mediaType switch
            {
                MediaType.Movie => new SearchMovie(),
                MediaType.Tv => new SearchTv(),
                MediaType.Person => new SearchPerson(),
                MediaType.Episode => new SearchTvEpisode(),
                MediaType.TvEpisode => new SearchTvEpisode(),
                MediaType.Season => new SearchTvSeason(),
                MediaType.TvSeason => new SearchTvSeason(),
                MediaType.Collection => new SearchCollection(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        if (result is not null)
        {
            using var jsonReader = jObject.CreateReader();
            serializer.Populate(jsonReader, result);
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        var jToken = JToken.FromObject(value);
        jToken.WriteTo(writer);
    }
}
