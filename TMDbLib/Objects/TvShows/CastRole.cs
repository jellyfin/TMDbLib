using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a role played by a cast member in a TV show.
/// </summary>
public class CastRole
{
    /// <summary>
    /// Gets or sets the character name.
    /// </summary>
    [JsonProperty("character")]
    public string Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string CreditId { get; set; }

    /// <summary>
    /// Gets or sets the number of episodes for this role.
    /// </summary>
    [JsonProperty("episode_count")]
    public int EpisodeCount { get; set; }
}
