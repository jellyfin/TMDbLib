using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class TvGroupNetwork : NetworkBase
    {
        [JsonProperty("logo_path")]
        public string LogoPath { get; set; }

        [JsonProperty("origin_country")]
        public string OriginCountry { get; set; }
    }
}