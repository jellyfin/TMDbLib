using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities.Converters
{
    internal class SearchBaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SearchBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            SearchBase result;
            if (jObject["media_type"] == null)
            {
                // We cannot determine the correct type, let's hope we were provided one
                result = (SearchBase)Activator.CreateInstance(objectType);
            }
            else
            {
                // Determine the type based on the media_type
                MediaType mediaType = jObject["media_type"].ToObject<MediaType>();

                result = mediaType switch
                {
                    MediaType.Movie => new SearchMovie(),
                    MediaType.Tv => new SearchTv(),
                    MediaType.Person => new SearchPerson(),
                    MediaType.Episode => new SearchTvEpisode(),
                    MediaType.TvEpisode => new SearchTvEpisode(),
                    MediaType.Season => new SearchTvSeason(),
                    MediaType.TvSeason => new SearchTvSeason(),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }

            // Populate the result
            using JsonReader jsonReader = jObject.CreateReader();
            serializer.Populate(jsonReader, result);

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(value);

            jToken.WriteTo(writer);
        }
    }
}