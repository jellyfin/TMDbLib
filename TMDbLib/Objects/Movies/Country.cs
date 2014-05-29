using System;
using Newtonsoft.Json;
using TMDbLib.Converters;

namespace TMDbLib.Objects.Movies
{
    public class Country
    {
        [JsonProperty("iso_3166_1")]
        public string Iso_3166_1 { get; set; }
        [JsonProperty("certification")]
        public string Certification { get; set; }
        [JsonProperty("release_date")]
        [JsonConverter(typeof(DateTimeConverterYearMonthDay))]
        public DateTime? ReleaseDate { get; set; }
    }
}