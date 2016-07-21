using Newtonsoft.Json;

namespace TMDbLib.Objects.General
{
    public class ExternalIds
    {
        [JsonProperty("freebase_id")]
        public string FreebaseId { get; set; }

        [JsonProperty("freebase_mid")]
        public string FreebaseMid { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty("tvdb_id")]
        public string TvdbId { get; set; }

        [JsonProperty("tvrage_id")]
        public string TvrageId { get; set; }
    }
}
