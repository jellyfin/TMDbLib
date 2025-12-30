using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents a cast member in a TV show or episode.
/// </summary>
public class Cast : CastBase
{
    /// <summary>
    /// Gets or sets the character name played by the cast member.
    /// </summary>
    [JsonProperty("character")]
    public string? Character { get; set; }

    /// <summary>
    /// Gets or sets the credit ID.
    /// </summary>
    [JsonProperty("credit_id")]
    public string? CreditId { get; set; }
}
