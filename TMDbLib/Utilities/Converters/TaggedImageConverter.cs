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

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);

        TaggedImage result = new TaggedImage();

        using (JsonReader jsonReader = jObject.CreateReader())
        {
            serializer.Populate(jsonReader, result);
        }

        JToken mediaJson = jObject["media"];
        result.Media = result.MediaType switch
        {
            MediaType.Movie => mediaJson.ToObject<SearchMovie>(),
            MediaType.Tv => mediaJson.ToObject<SearchTv>(),
            MediaType.Episode => mediaJson.ToObject<SearchTvEpisode>(),
            MediaType.Season => mediaJson.ToObject<SearchTvSeason>(),
            _ => throw new ArgumentOutOfRangeException(),
        };
        return result;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JToken jToken = JToken.FromObject(value);

        jToken.WriteTo(writer);
    }
}
