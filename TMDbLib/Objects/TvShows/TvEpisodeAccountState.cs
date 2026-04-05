using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents the account state for a TV episode.
/// </summary>
public class TvEpisodeAccountState : TvAccountState
{
    /// <summary>
    /// Gets or sets the TMDb if for the related movie.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
