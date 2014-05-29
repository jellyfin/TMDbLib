using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class ExternalIds
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }
        [JsonProperty("freebase_id")]
        public string FreebaseId { get; set; }
        [JsonProperty("freebase_mid")]
        public string FreebaseMid { get; set; }
        [JsonProperty("tvdb_id")]
        public int? TvdbId { get; set; }
        [JsonProperty("tvrage_id")]
        public int? TvrageId { get; set; }
    }
}