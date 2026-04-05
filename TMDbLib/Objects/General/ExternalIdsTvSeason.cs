using System.Text.Json.Serialization;

namespace TMDbLib.Objects.General;

/// <summary>
/// Represents external IDs for a TV season.
/// </summary>
public class ExternalIdsTvSeason : ExternalIds
{
    /// <summary>
    /// Gets or sets the TVDb ID.
    /// </summary>
    [JsonPropertyName("tvdb_id")]
    public int? TvdbId { get; set; }
}
