using Newtonsoft.Json;

namespace TMDbLib.Objects.Search
{
    public class SearchCollection
    {
        [JsonProperty("adult")]
        public bool Adult { get; set; }
        
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        
        [JsonProperty("original_name")]
        public string OriginalName { get; set; }
        
        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }
}