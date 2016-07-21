using System;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.Search;

namespace TMDbLib.Utilities
{
    internal class KnownForConverter : JsonCreationConverter<KnownForBase>
    {
        protected override KnownForBase GetInstance(JObject jObject)
        {
            string mediaType = jObject["media_type"].ToString();

            switch (mediaType)
            {
                case "movie":
                    return new KnownForMovie();
                case "tv":
                    return new KnownForTv();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(KnownForBase);
        }
    }
}