using System.Text.Json.Serialization;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.TvShows;

/// <summary>
/// Represents the account state for a TV show or episode.
/// </summary>
[JsonDerivedType(typeof(TvEpisodeAccountState))]
[JsonDerivedType(typeof(TvEpisodeAccountStateWithNumber))]
public class TvAccountState
{
    /// <summary>
    /// Gets or sets the user rating.
    /// </summary>
    [JsonPropertyName("rated")]
    [JsonConverter(typeof(TmdbRatingConverterFactory))]
    public double? Rating { get; set; }
}
