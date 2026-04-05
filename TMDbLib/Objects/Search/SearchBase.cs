using System.Text.Json.Serialization;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;

namespace TMDbLib.Objects.Search;

/// <summary>
/// Base class for all search results.
/// </summary>
[JsonDerivedType(typeof(SearchCollection))]
[JsonDerivedType(typeof(SearchMovieTvBase))]
[JsonDerivedType(typeof(SearchPerson))]
[JsonDerivedType(typeof(SearchTvEpisode))]
[JsonDerivedType(typeof(SearchTvSeason))]
[JsonDerivedType(typeof(SearchMovieWithRating))]
[JsonDerivedType(typeof(SearchMovie))]
[JsonDerivedType(typeof(SearchTv))]
[JsonDerivedType(typeof(FindPerson))]
[JsonDerivedType(typeof(AccountSearchTv))]
[JsonDerivedType(typeof(SearchTvShowWithRating))]
[JsonConverter(typeof(SearchBaseConverter))]
public class SearchBase
{
    /// <summary>
    /// Gets or sets the TMDb ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the media type.
    /// </summary>
    [JsonPropertyName("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Gets or sets the popularity score.
    /// </summary>
    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }
}
