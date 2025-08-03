using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

public class CastRole
{
    [JsonProperty("character")]
    public string Character { get; set; }

    [JsonProperty("credit_id")]
    public string CreditId { get; set; }

    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }
}
