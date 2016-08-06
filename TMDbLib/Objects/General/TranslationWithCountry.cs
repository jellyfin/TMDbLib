using Newtonsoft.Json;

namespace TMDbLib.Objects.Movies
{
    public class TranslationWithCountry : Translation
    {
        /// <summary>
        /// A country code, e.g. US
        /// </summary>
        [JsonProperty("iso_3166_1")]
        public string Iso_3166_1 { get; set; }
    }
}