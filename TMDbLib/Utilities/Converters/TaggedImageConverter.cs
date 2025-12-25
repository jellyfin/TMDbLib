using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters;

internal class TaggedImageConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TaggedImage);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var result = new TaggedImage();

        using (JsonReader jsonReader = jObject.CreateReader())
        {
            serializer.Populate(jsonReader, result!);
        }

        var mediaJson = jObject["media"];
        if (mediaJson is not null)
        {
            result.Media = result.MediaType switch
            {
                MediaType.Movie => mediaJson.ToObject<SearchMovie>(),
                MediaType.Tv => mediaJson.ToObject<SearchTv>(),
                MediaType.Episode => mediaJson.ToObject<SearchTvEpisode>(),
                MediaType.Season => mediaJson.ToObject<SearchTvSeason>(),
                _ => throw new ArgumentOutOfRangeException(),
            };
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
