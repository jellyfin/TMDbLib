using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

public class Cast : CastBase
{
    [JsonProperty("character")]
    public string Character { get; set; }

    [JsonProperty("credit_id")]
    public string CreditId { get; set; }
}
