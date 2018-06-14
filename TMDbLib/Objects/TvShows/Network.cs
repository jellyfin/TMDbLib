using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class Network : NetworkBase
    {
        [JsonProperty("headquarters")]
        public string Headquarters;

        [JsonProperty("homepage")]
        public string Homepage;

        [JsonProperty("origin_country")]
        public string OriginCountry;
    }
}