using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a TV episode.
/// </summary>
public class ExternalIdsTvEpisode : ExternalIds
{
    /// <summary>
    /// Gets or sets the IMDb ID.
    /// </summary>
    [JsonProperty("imdb_id")]
    public string ImdbId { get; set; }

    /// <summary>
    /// Gets or sets the TVDb ID.
    /// </summary>
    [JsonProperty("tvdb_id")]
    public string TvdbId { get; set; }
}
