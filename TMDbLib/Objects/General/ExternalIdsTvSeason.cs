using Newtonsoft.Json;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a TV season.
/// </summary>
public class ExternalIdsTvSeason : ExternalIds
{
    /// <summary>
    /// Gets or sets the TVDb ID.
    /// </summary>
    [JsonProperty("tvdb_id")]
    public string TvdbId { get; set; }
}
